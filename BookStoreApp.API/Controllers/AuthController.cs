using AutoMapper;
using BookStoreApp.API.Constants;
using BookStoreApp.API.Data;
using BookStoreApp.API.DTOs.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly IMapper mapper;
        private readonly UserManager<ApiUser> userManager;
        private readonly IConfiguration configuration;

        public AuthController(ILogger<AuthController> logger, IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserRegDTO userRegDTO)
        {
            if (userRegDTO == null)
            {
                return BadRequest("Insufficient data provided.");
            }

            try
            {
                var user = mapper.Map<ApiUser>(userRegDTO);
                user.UserName = userRegDTO.Email;
                user.NormalizedUserName = userRegDTO.Email.ToUpper();
                user.NormalizedEmail = userRegDTO.Email.ToUpper();
                var res = await userManager.CreateAsync(user, userRegDTO.Password);
                if (!res.Succeeded)
                {
                    foreach(var error in res.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    logger.LogError($"POST - {nameof(Register)} - Error registering user: {userRegDTO.ToString}.");
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrEmpty(userRegDTO.Role))
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
                else
                {
                    await userManager.AddToRoleAsync(user, userRegDTO.Role);
                }

                return Ok();

            } catch (Exception ex)
            {
                logger.LogError(ex, $"POST - {nameof(Register)} - Error registering user: {userRegDTO.Email}.");
                return StatusCode(500, StatusCodeMessages.Error500Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthResponse>> Login(UserLoginDTO userLoginDTO)
        {
            logger.LogInformation($"Login attempt for {userLoginDTO.Email}");

            try
            {

                var user = await userManager.FindByEmailAsync(userLoginDTO.Email);
                var passwordValid = await userManager.CheckPasswordAsync(user, userLoginDTO.Password);
                
                if (user == null || !passwordValid)
                {
                    return Unauthorized();
                }

                string tokenString = await GenerateToken(user);

                var response = new AuthResponse
                {
                    Email = user.Email,
                    Token = tokenString,
                    UserId = user.Id,
                };

                return Accepted(response);


            } catch (Exception ex)
            {
                logger.LogError(ex, $"POST - {nameof(Login)} - Error logging in user: {userLoginDTO.Email}.");
                return Problem(StatusCodeMessages.Error500Message, statusCode: 500);
            }
        }

        private async Task<string> GenerateToken(ApiUser user)
        {
            if (user == null) { return null; };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q));
            var userClaims = await userManager.GetClaimsAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }.Union(roleClaims)
            .Union(userClaims);

            var token = new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(configuration["JwtSettings:Duration"])),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
