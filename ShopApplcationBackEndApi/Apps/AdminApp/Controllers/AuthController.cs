using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApplcationBackEndApi.Apps.AdminApp.Dtos.UserDto;
using ShopApplcationBackEndApi.Entities;

namespace ShopApplcationBackEndApi.Apps.AdminApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser > _userManager;

        public AuthController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var existUser = _userManager.FindByNameAsync(registerDto.UserName);
            if (existUser != null) return BadRequest();
            AppUser appUser = new AppUser();
            appUser.UserName = registerDto.UserName;
            appUser.Email = registerDto.Email;
            appUser.FullName=registerDto.FullName;
            var result= await _userManager.CreateAsync(appUser,registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            await _userManager.AddToRoleAsync(appUser, "member");
            return StatusCode(StatusCodes.Status201Created);
        }
        
    }
}
