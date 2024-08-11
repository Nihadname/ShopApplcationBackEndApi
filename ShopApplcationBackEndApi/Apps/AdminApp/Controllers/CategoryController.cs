using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApplcationBackEndApi.Apps.AdminApp.Dtos.CategoryDto;
using ShopApplcationBackEndApi.Data;
using ShopApplcationBackEndApi.Entities;

namespace ShopApplcationBackEndApi.Apps.AdminApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ShopAppContext shopAppContext;

        public CategoryController(ShopAppContext shopAppContext)
        {
            this.shopAppContext = shopAppContext;
        }
        [HttpGet("{Id}")] 
        public async Task<IActionResult> Get(int? Id)
        {
            if (Id == null) return BadRequest();
            var existedCategory=await shopAppContext.categories.Where(s=>!s.IsDeleted).FirstOrDefaultAsync(c => c.Id == Id);
            if(existedCategory == null) return NotFound();
            CategoryReturnDto categoryReturnDto = new CategoryReturnDto();
            categoryReturnDto.Name = existedCategory.Name;
            categoryReturnDto.Id = existedCategory.Id;
            categoryReturnDto.CreatedDate=existedCategory.CreatedTime;
            categoryReturnDto.UpdateDate=existedCategory.UpdatedTime;
            categoryReturnDto.ImageUrl= "http://localhost:5104/img/"+existedCategory.Image;
            return Ok(categoryReturnDto);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto categoryCreateDto)
        {
            var Isexisted = await shopAppContext.categories.AnyAsync(s => !s.IsDeleted && s.Name.ToLower() == categoryCreateDto.Name.ToLower());

            if (Isexisted) return StatusCode(StatusCodes.Status409Conflict);
            Category category =new Category();
            if(categoryCreateDto.Photo is  null) return BadRequest();
            if (!categoryCreateDto.Photo.ContentType.Contains("image/")) return BadRequest();
            if(categoryCreateDto.Photo.Length>500*1024) return BadRequest();
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(categoryCreateDto.Photo.FileName);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);
            using FileStream fileStream = new(path, FileMode.Create);
            await categoryCreateDto.Photo.CopyToAsync(fileStream);
            category.Name = categoryCreateDto.Name.Trim();
            category.Image = fileName;
        await shopAppContext.AddAsync(category);
            await shopAppContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
