using law_firm_management.Dto.AccountDto;
using law_firm_management.interfaces;
using law_firm_management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace law_firm_management.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userManager.FindByEmailAsync(loginDto.Email);

                if (user == null)
                {
                    Log.Warning($"Login attempt failed for non-existent user: {loginDto.Email}");
                    return Unauthorized("Invalid email or password.");
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                if (!result.Succeeded)
                {
                    Log.Warning($"Login attempt failed for user: {loginDto.Email}");
                    return Unauthorized("Invalid email or password.");
                }

                Log.Information($"User logged in successfully: {loginDto.Email}");
                return Ok(new UserDto
                {
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error during login process");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingUser = await _userManager.FindByEmailAsync(registerDto.Email.ToLower());
                if (existingUser != null)
                {
                    Log.Warning($"Registration attempt failed - Email already exists: {registerDto.Email}");
                    return Conflict("Email already exists.");
                }

                var appUser = new AppUser
                {
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    PhoneNumber = registerDto.PhoneNumber,
                    Email = registerDto.Email,
                    UserName = registerDto.Email // Set email as the UserName
                };

                var createUserResult = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (createUserResult.Succeeded)
                {
                    var addToRoleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (!addToRoleResult.Succeeded)
                    {
                        Log.Error($"Failed to assign role to user: {appUser.Email}");
                        return StatusCode(500, "Failed to assign role to user.");
                    }

                    Log.Information($"User registered successfully: {registerDto.Email}");
                    return Ok(new UserDto
                    {
                        Email = appUser.Email,
                        Token = _tokenService.CreateToken(appUser)
                    });
                }
                else
                {
                    Log.Error($"Failed to create user: {registerDto.Email}");
                    foreach (var error in createUserResult.Errors)
                    {
                        if (error.Code == "InvalidUserName")
                        {
                            ModelState.AddModelError(nameof(RegisterDto.Email), "Invalid email format.");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error during registration process");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}