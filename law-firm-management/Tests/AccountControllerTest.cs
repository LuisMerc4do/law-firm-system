using System.Threading.Tasks;
using law_firm_management.Controllers;
using law_firm_management.Dto.AccountDto;
using law_firm_management.interfaces;
using law_firm_management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace law_firm_management.Tests
{
    public class AccountControllerTests
    {
        private readonly Mock<UserManager<AppUser>> _mockUserManager;
        private readonly Mock<SignInManager<AppUser>> _mockSignInManager;
        private readonly Mock<ITokenService> _mockTokenService;

        public AccountControllerTests()
        {
            _mockUserManager = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
            _mockSignInManager = new Mock<SignInManager<AppUser>>(_mockUserManager.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(), null, null, null, null);
            _mockTokenService = new Mock<ITokenService>();
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsOkResult()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@example.com", Password = "password" };
            var user = new AppUser { Email = loginDto.Email, UserName = loginDto.Email };

            _mockUserManager.Setup(x => x.FindByEmailAsync(loginDto.Email)).ReturnsAsync(user);
            _mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(user, loginDto.Password, false)).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            _mockTokenService.Setup(x => x.CreateToken(user)).Returns("test-token");

            var controller = new AccountController(_mockUserManager.Object, _mockTokenService.Object, _mockSignInManager.Object);

            // Act
            var result = await controller.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var userDto = Assert.IsType<UserDto>(okResult.Value);
            Assert.Equal(loginDto.Email, userDto.Email);
            Assert.Equal("test-token", userDto.Token);
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "test@example.com", Password = "wrongpassword" };
            var user = new AppUser { Email = loginDto.Email, UserName = loginDto.Email };

            _mockUserManager.Setup(x => x.FindByEmailAsync(loginDto.Email)).ReturnsAsync(user);
            _mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(user, loginDto.Password, false)).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            var controller = new AccountController(_mockUserManager.Object, _mockTokenService.Object, _mockSignInManager.Object);

            // Act
            var result = await controller.Login(loginDto);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Register_WithNewUser_ReturnsOkResult()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Email = "newuser@example.com",
                Password = "password",
                FirstName = "New",
                LastName = "User",
                PhoneNumber = "1234567890"
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(registerDto.Email)).ReturnsAsync((AppUser)null);
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), registerDto.Password)).ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), "User")).ReturnsAsync(IdentityResult.Success);
            _mockTokenService.Setup(x => x.CreateToken(It.IsAny<AppUser>())).Returns("test-token");

            var controller = new AccountController(_mockUserManager.Object, _mockTokenService.Object, _mockSignInManager.Object);

            // Act
            var result = await controller.Register(registerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var userDto = Assert.IsType<UserDto>(okResult.Value);
            Assert.Equal(registerDto.Email, userDto.Email);
            Assert.Equal("test-token", userDto.Token);
        }

        [Fact]
        public async Task Register_WithExistingEmail_ReturnsConflict()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Email = "existing@example.com",
                Password = "password",
                FirstName = "Existing",
                LastName = "User",
                PhoneNumber = "1234567890"
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(registerDto.Email)).ReturnsAsync(new AppUser());

            var controller = new AccountController(_mockUserManager.Object, _mockTokenService.Object, _mockSignInManager.Object);

            // Act
            var result = await controller.Register(registerDto);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
        }
    }
}