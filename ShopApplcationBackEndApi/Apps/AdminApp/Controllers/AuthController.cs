using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShopApplcationBackEndApi.Apps.AdminApp.Dtos.UserDto;
using ShopApplcationBackEndApi.Entities;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ShopApplcationBackEndApi.Apps.AdminApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser > _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AuthController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var existUser =await _userManager.FindByNameAsync(registerDto.UserName);
            if (existUser != null) return BadRequest();
            AppUser appUser = new AppUser();
            appUser.UserName = registerDto.UserName;
            appUser.Email = registerDto.Email;
            appUser.FullName = registerDto.FullName;
            var result = await _userManager.CreateAsync(appUser, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            await _userManager.AddToRoleAsync(appUser, "Member");
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPost("LogIn")]
public async Task<IActionResult> LogIn(LoginDto loginDto)
        {
            var existUser = await _userManager.FindByNameAsync(loginDto.UserName);
            if (existUser == null) return BadRequest();
            var result=await _userManager.CheckPasswordAsync(existUser, loginDto.Password);
            if (!result)
            {
                return BadRequest();
            }
             var handler= new JwtSecurityTokenHandler();
            var privateKey= Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:SecretKey").Value);
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(privateKey), SecurityAlgorithms.HmacSha256);
            var ci = new ClaimsIdentity();

            ci.AddClaim(new Claim(ClaimTypes.NameIdentifier, existUser.Id));
            ci.AddClaim(new Claim(ClaimTypes.Name, existUser.UserName));
            ci.AddClaim(new Claim(ClaimTypes.GivenName, existUser.FullName));
            ci.AddClaim(new Claim(ClaimTypes.Email, existUser.Email));
            var roles=await _userManager.GetRolesAsync(existUser);
           ci.AddClaims(roles.Select(r=> new Claim(ClaimTypes.Role, r)).ToList());     
            //foreach (var role in roles)
            //    ci.AddClaim(new Claim(ClaimTypes.Role, role));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(1),
                Subject = ci
            };
            var tokenHandiling = handler.CreateToken(tokenDescriptor);
            var Token= handler.WriteToken(tokenHandiling);

            return Ok(new {token= Token });

        }
    }
}
