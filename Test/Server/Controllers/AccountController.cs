using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Test.Server.Models;
using Test.Shared.Entities.DTO;

namespace Test.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettings;
        public AccountController(ILogger<AccountController> logger, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _configuration = configuration;
            _jwtSettings = _configuration.GetSection("JwtSettings");
            _logger = logger;
            _userManager = userManager;
        }

        public IClientRequestParametersProvider ClientRequestParametersProvider { get; }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserAuthenticationRequest authUser)
        {

                var user = await _userManager.FindByNameAsync(authUser.Email);
                if (user == null || !await _userManager.CheckPasswordAsync(user, authUser.Password))
                    return Unauthorized(new AuthenticationResponse { ErrorMessage = "Autenticación no es válida" });
                var signingCredentials = GetUserCredentials();
                var claims = GetClaims(user);
                var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new AuthenticationResponse { IsAuthSuccessful = true, Token = token });
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterRequest regUser)
        {
            if (regUser == null || !ModelState.IsValid)
                return BadRequest();

            var user = new ApplicationUser { UserName = regUser.Email, Email = regUser.Email };

            var result = await _userManager.CreateAsync(user, regUser.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return BadRequest(new RegisterResponse { Errors = errors });
            }

            return StatusCode(201);
        }
        private SigningCredentials GetUserCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings["securityKey"]);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private List<Claim> GetClaims(IdentityUser user)
        {
            var claims = new List<Claim>{
                new Claim(ClaimTypes.Name, user.Email)
            };

            return claims;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings["validIssuer"],
                audience: _jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings["expiryInMinutes"])),
                signingCredentials: signingCredentials);

            return tokenOptions;
        }
    }
}