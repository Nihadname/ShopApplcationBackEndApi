using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShopApplcationBackEndApi.Apps.AdminApp.Dtos.UserDto;
using ShopApplcationBackEndApi.Entities;
using ShopApplcationBackEndApi.Services.Interfaces;
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
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        public AuthController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IMapper mapper, ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
            _tokenService = tokenService;
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
           
            IList<string> roles=await _userManager.GetRolesAsync(existUser);
            var Audience=_configuration.GetSection("Jwt:Audience").Value;
            var SecretKey = _configuration.GetSection("Jwt:secretKey").Value;
            var Issuer=_configuration.GetSection("Jwt:Issuer").Value;
            return Ok(new {token= _tokenService.GetToken(SecretKey, Audience, Issuer, existUser,roles) });

        }
        [HttpGet]
        [Authorize(Roles = "Member")]
        public async  Task<IActionResult> UserProfile()
        {
            var existedUser= await _userManager.GetUserAsync(User);
            if (existedUser==null) return NotFound();

            return Ok(_mapper.Map<UserGetDto>(existedUser));
        }
    }
}
