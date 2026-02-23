using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using GymManagement.Models.DTOs;

namespace GymManagement.Controllers.Api
{
    /// <summary>
    /// API controller for authentication (login and registration) using JWT tokens.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        public AuthController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="model">The login credentials.</param>
        /// <returns>A JWT token response on success, or 401 Unauthorized on failure.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ApiLoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new { message = "Invalid email or password." });

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
                return Unauthorized(new { message = "Invalid email or password." });

            var token = await GenerateJwtToken(user);
            return Ok(token);
        }

        /// <summary>
        /// Registers a new user with the "User" role and returns a JWT token.
        /// </summary>
        /// <param name="model">The registration details.</param>
        /// <returns>A JWT token response on success, or 400 BadRequest on failure.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ApiRegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            // Assign the default "User" role to newly registered users.
            await _userManager.AddToRoleAsync(user, "User");

            var token = await GenerateJwtToken(user);
            return Ok(token);
        }

        /// <summary>
        /// Generates a JWT token for the specified user, including their roles as claims.
        /// </summary>
        private async Task<ApiTokenResponseDto> GenerateJwtToken(IdentityUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add each role as a claim so [Authorize(Roles = "...")] works with JWT.
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"] ?? "60");
            var expiration = DateTime.UtcNow.AddMinutes(expireMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new ApiTokenResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Email = user.Email!,
                Roles = roles
            };
        }
    }
}
