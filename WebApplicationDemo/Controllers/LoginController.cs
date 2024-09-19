using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplicationDemo.Model;
using WebApplicationDemo.Util;

namespace WebApplicationDemo.Controllers
{
    [ApiController]
    [Route("api")]
    public class LoginController : ControllerBase
    {

        private readonly ILogger<LoginController> _logger;
        private readonly DemoContext _context;
        private readonly IConfiguration _configuration;
        private readonly JwtBlacklistService _jwtBlacklistService;

        public LoginController(DemoContext context, IConfiguration configuration, JwtBlacklistService jwtBlacklistService, ILogger<LoginController> logger)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            _jwtBlacklistService = jwtBlacklistService;
        }

        [Route("register")]
        [HttpPost]
        public ActionResult Register(UserViewModel userViewModel)
        {
            if (string.IsNullOrEmpty(userViewModel.username) || string.IsNullOrEmpty(userViewModel.password) )
            {
                return BadRequest("empty input");
            }
            if (!PasswordHelper.ContainsSpecialCharacter(userViewModel.password))
            {
                return BadRequest("password should contain special character: #, $, @, !, %, &");
            }
            var user = _context.userData.FirstOrDefault<UserDataModel>(u => u.username == userViewModel.username);
            if (user != null)
            {
                return BadRequest("user exist");
            }
            UserDataModel userModel = new UserDataModel()
            {
                username = userViewModel.username,
                password = PasswordHelper.HashPassword(userViewModel.password)
            };
            _context.userData.Add(userModel);
            int affectedRows = _context.SaveChanges();
            if (affectedRows > 0)
            {
                return Created("register", "");
            }
            else
            {
                return BadRequest("unhandle exception");
            }
        }

        [Route("login")]
        [HttpPost]
        public ActionResult<string> Login(UserViewModel userViewModel)
        {
            var user = _context.userData.FirstOrDefault<UserDataModel>(u => u.username == userViewModel.username);
            if (user == null)
            {
                return BadRequest("user not exist");
            }
            string hashedPassword = user.password;
            if (PasswordHelper.VerifyPassword(userViewModel.password, hashedPassword) == false )
            {
                return BadRequest("password error");
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            List<Claim> claimList = new List<Claim>();
            claimList.Add(new Claim(ClaimTypes.Name, user.username));
            var claims = claimList.ToArray<Claim>();

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                notBefore: DateTime.Now,
                 expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
            );
            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            HttpContext.Session.SetString(user.username, "");

            return Ok(jwtToken);
        }

        [Route("change-password")]
        [HttpPost]
        [Authorize]
        public ActionResult<string> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            string userName = User.Identity?.Name == null ? "" : User.Identity.Name;
            var user = _context.userData.FirstOrDefault<UserDataModel>(u => u.username == userName);
            if (user == null)
            {
                return BadRequest("user not exist");
            }
            if (changePasswordViewModel.oldPassword == changePasswordViewModel.newPassword)
            {
                return BadRequest("password the same");
            }

            user.password = PasswordHelper.HashPassword(changePasswordViewModel.newPassword);
            int affectedRows = _context.SaveChanges();
            if (affectedRows > 0)
            {
                return Ok("change success");
            }
            else
            {
                return BadRequest("change not success");
            }
        }

        [Route("logout")]
        [HttpPost]
        [Authorize]
        public ActionResult Logout()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            _jwtBlacklistService.BlacklistToken(token);
            string userName = User.Identity?.Name == null ? "" : User.Identity.Name;
            HttpContext.Session.Remove(userName);
            return Ok();
        }

        [Route("profile")]
        [HttpGet]
        [Authorize]
        public ActionResult Profile()
        {
            string userName = User.Identity?.Name == null ? "" : User.Identity.Name;
            UserProfile userProfile = new UserProfile()
            {
                email = "",
                fullName = "",
                username = userName,
            };
            return Ok(userProfile);
        }

        [Route("userData")]
        [HttpGet]
        [Authorize]
        public ActionResult<List<UserViewModel>> UserData()
        {
            List<UserViewModel> list = new List<UserViewModel>();
            var users = _context.userData.ToList();
            foreach (var user in users)
            {
                UserViewModel userViewModel = new UserViewModel()
                {
                    username = user.username,
                    password = user.password
                };
                list.Add(userViewModel);
            }

            return Ok(list);
        }

        [Route("health")]
        [HttpGet]
        public ActionResult Health()
        {
            return Ok();
        }
    }
}
