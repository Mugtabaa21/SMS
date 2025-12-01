using mewo.Data;
using mewo.Dtos;
using mewo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace mewo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration config, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _config = config;
            _roleManager = roleManager;
        }

        // GOAL: Create new user + Assign Role
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto register)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var appUser = new ApplicationUser
            {
                UserName = register.Username,
                Email = register.Email
            };

            var result = await _userManager.CreateAsync(appUser, register.Password);

            if (result.Succeeded)
            {
                string roleName = register.Role.ToString();

                // Auto-create role if it doesn't exist
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }

                await _userManager.AddToRoleAsync(appUser, roleName);

                return Ok(appUser);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        // GOAL: Authenticate user + Generate Token
        [HttpPost("Login")]
        public async Task<IActionResult> Login(SignInDto login)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? user = await _userManager.FindByNameAsync(login.Username);
                if (user != null)
                {
                    if (await _userManager.CheckPasswordAsync(user, login.Password))
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, login.Username),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var roles = await _userManager.GetRolesAsync(user);
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]));
                        var sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            claims: claims,
                            issuer: _config["JWT:Issuer"],
                            audience: _config["JWT:Audiance"],
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: sc
                        );

                        var _token = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expire = token.ValidTo,
                        };
                        return Ok(_token);
                    }
                    return Unauthorized();
                }
                ModelState.AddModelError("", "User name is invalid!");
            }
            return BadRequest(ModelState);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("ChangeRole")]
        public async Task<IActionResult> AssignRole([FromForm] UserRole model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            string roleName = model.Role.ToString();

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok($"Role '{roleName}' assigned to user '{model.Username}'.");
            }
            else
            {
                return BadRequest("Error assigning role to user.");
            }
        }
    }
}