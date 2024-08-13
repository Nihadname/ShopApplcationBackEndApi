using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApplcationBackEndApi.Apps.AdminApp.Dtos.CategoryDto;
using ShopApplcationBackEndApi.Data;
using ShopApplcationBackEndApi.Entities;
using System.IO;
using System.Security.Authentication.ExtendedProtection;

namespace ShopApplcationBackEndApi.Apps.AdminApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ShopAppContext shopAppContext;
        private readonly IMapper mapper;
        public CategoryController(ShopAppContext shopAppContext, IMapper mapper)
        {
            this.shopAppContext = shopAppContext;
            this.mapper = mapper;
        }
        [HttpGet]

        public async Task<IActionResult> Get(string search, int page = 1)
        {
            var query = shopAppContext.categories.Where(p => !p.IsDeleted);
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Name.ToLower().Contains(search.ToLower()));
            }
            categoryListDto categoryListDto = new()
            {
                Page = page,
                TotalCount = query.Count(),
                categories = await query.Skip((page - 1) * 2).Take(2)
                         .Select(c => new CategoryListItemDto()
                         {
                             Id = c.Id,
                             Name = c.Name,
                             CreatedDate = c.CreatedTime,
                             UpdateDate = c.UpdatedTime,

                         }).ToListAsync()
            };
            return Ok(categoryListDto);

        }
        [HttpGet("{Id}")] 
        public async Task<IActionResult> Get(int? Id)
        {
            if (Id == null) return BadRequest();
            var existedCategory=await shopAppContext.categories.Where(s=>!s.IsDeleted).FirstOrDefaultAsync(c => c.Id == Id);
            if(existedCategory == null) return NotFound();
            var categoryReturnDto = mapper.Map<CategoryReturnDto>(existedCategory);
            return Ok(categoryReturnDto);
        }
        [HttpPost]
    
        public async Task<IActionResult> Create([FromForm]CategoryCreateDto categoryCreateDto)
        {
            var Isexisted = await shopAppContext.categories.AnyAsync(s => !s.IsDeleted && s.Name.ToLower() == categoryCreateDto.Name.ToLower());

            if (Isexisted) return StatusCode(StatusCodes.Status409Conflict);
            Category category =new Category();
            if(categoryCreateDto.Photo is  null) return BadRequest();
            if (!categoryCreateDto.Photo.ContentType.Contains("image/")) return BadRequest();
            if(categoryCreateDto.Photo.Length/ 1024 > 1500) return BadRequest();
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(categoryCreateDto.Photo.FileName);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);
            using FileStream fileStream = new(path, FileMode.Create);
            await categoryCreateDto.Photo.CopyToAsync(fileStream);
            category.Name = categoryCreateDto.Name.Trim();
            category.Image = fileName;
        await shopAppContext.categories.AddAsync(category); 
            await shopAppContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(int? Id ,CategoryUpdateDto categoryUpdateDto)
        {
            if (Id == null) return BadRequest();
            var existedCategory = await shopAppContext.categories.Where(s => !s.IsDeleted).FirstOrDefaultAsync(c => c.Id == Id);
            if (existedCategory == null) return NotFound();
            var Isexisted = await shopAppContext.categories.AnyAsync(s => !s.IsDeleted && s.Name.ToLower() == categoryUpdateDto.Name.ToLower()&&existedCategory.Name.ToLower()!=categoryUpdateDto.Name.ToLower());
            if (Isexisted) return StatusCode(StatusCodes.Status409Conflict);
            var photo = categoryUpdateDto.Photo;
            if (photo is null) return BadRequest();
            if (!photo.ContentType.Contains("image/")) return BadRequest();
            if (photo.Length / 1024 > 1500) return BadRequest();
            if (!string.IsNullOrEmpty(existedCategory.Image))
            {
                if (System.IO.File.Exists(existedCategory.Image))
                {
                    System.IO.File.Delete(existedCategory.Image);
                }
            }
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(categoryUpdateDto.Photo.FileName);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);
            using FileStream fileStream = new(path, FileMode.Create);
            await categoryUpdateDto.Photo.CopyToAsync(fileStream);
                existedCategory.Name=categoryUpdateDto.Name.Trim();
            existedCategory.Image = fileName;
             shopAppContext.categories.Update(existedCategory);
            await shopAppContext.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            var existedCategory = await shopAppContext.categories
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existedCategory == null) return NotFound();

            if (!string.IsNullOrEmpty(existedCategory.Image) && System.IO.File.Exists(existedCategory.Image))
            {
                System.IO.File.Delete(existedCategory.Image);
            }

            shopAppContext.categories.Remove(existedCategory);
            await shopAppContext.SaveChangesAsync();

            return Ok();
        }


    }
}
