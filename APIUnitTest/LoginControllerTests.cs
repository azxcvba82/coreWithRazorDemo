using Moq;
using Microsoft.AspNetCore.Mvc;
using WebApplicationDemo.Model;
using Microsoft.Extensions.Configuration;
using WebApplicationDemo.Controllers;
using Microsoft.Extensions.Logging;
namespace APIUnitTest
{
    public class LoginControllerTests
    {

        private readonly Mock<DemoContext> _dbContextMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly LoginController _controller;
        private readonly Mock<JwtBlacklistService> _jwtBlacklistService;
        private readonly Mock<ILogger<LoginController>> _logger;

        public LoginControllerTests()
        {
            _dbContextMock = new Mock<DemoContext>();

            _configurationMock = new Mock<IConfiguration>();
            _jwtBlacklistService = new Mock<JwtBlacklistService>();
            _logger = new Mock<ILogger<LoginController>>();

            _controller = new LoginController(
                _dbContextMock.Object,
                _configurationMock.Object,
                _jwtBlacklistService.Object,
                _logger.Object
            );
        }

        [Fact]
        public void Health_ReturnsOk()
        {
            // Arrange skip

            // Act
            var result = _controller.Health();

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}