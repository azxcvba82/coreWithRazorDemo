using Microsoft.AspNetCore.Mvc;
using MVCRazor.Models;

namespace MVCRazor.Controllers
{
    public class AccountController : Controller
    {
        // In-memory user storage for simplicity demo.
        private static readonly List<RegisterViewModel> RegisteredUsers = new List<RegisterViewModel>();

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (RegisteredUsers.Any(u => u.username == model.username))
                {
                    ModelState.AddModelError("", "Username already exists.");
                    return View(model);
                }

                RegisteredUsers.Add(model);
                return RedirectToAction("Login");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = RegisteredUsers.SingleOrDefault(u => u.username == model.username && u.password == model.password);

                if (user != null)
                {
                    TempData["Username"] = model.username;
                    HttpContext.Session.SetString("Username", model.username);
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            var username = HttpContext.Session.GetString("Username");
            if (TempData["Username"] != null)
            {
                ViewBag.Username = TempData["Username"].ToString();
                return View();
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
