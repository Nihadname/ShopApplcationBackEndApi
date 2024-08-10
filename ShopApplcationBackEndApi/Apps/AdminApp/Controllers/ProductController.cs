using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApplcationBackEndApi.Apps.AdminApp.Dtos.ProductDto;
using ShopApplcationBackEndApi.Data;
using ShopApplcationBackEndApi.Entities;
using System.Collections.Generic;

namespace ShopApplcationBackEndApi.Apps.AdminApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ShopAppContext _shopAppContext;

        public ProductController(ShopAppContext shopAppContext)
        {
            _shopAppContext = shopAppContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get(string search,int page=1)
        {
            var productsAsQuery = _shopAppContext.Products.AsNoTracking().Where(s => !s.IsDeleted);
            if (!string.IsNullOrEmpty(search))
            {
                productsAsQuery = productsAsQuery.Where(s => s.Name.ToLower().Contains(search.ToLower()));
            }
            ProductListDto productListDto = new ProductListDto();
            productListDto.Page = page;
            productListDto.TotalCount=await productsAsQuery.CountAsync();
            productListDto.Items = await productsAsQuery.Skip((page - 1) * 2).Take(2).Select(s => new ProductListItemDto { 
                Name = s.Name,
                Id = s.Id,
                CostPrice= s.CostPrice,
                SalePrice= s.SalePrice,
                CreatedTime=s.CreatedTime,
                UpdatedTime=s.UpdatedTime,
                ProfitMadeFromOne=(int)(s.SalePrice-s.CostPrice),
                Category=new CategoryInProductListItemDto()
                {
                    Name = s.Category.Name,
                    ProductCount = s.Category.Products.Count(),
                }
            }).ToListAsync();

            return Ok(productListDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int? id)
        {
            if (id == null) return BadRequest("id can never be null");
            var product = await _shopAppContext.Products.Include(s=>s.Category).ThenInclude(s=>s.Products)
                .Where(s => !s.IsDeleted).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        
            if (product == null)
            {
                return NotFound();
            }
            ProductReturnDto returnDto = new ProductReturnDto();
            returnDto.Name = product.Name;
            returnDto.SalePrice = product.SalePrice;
            returnDto.CostPrice = product.CostPrice;
            returnDto.CreatedTime = product.CreatedTime;
            returnDto.UpdatedTime = product.UpdatedTime;
            returnDto.Id = product.Id;
            returnDto.ProfitMadeFromOne = (int)(product.SalePrice - product.CostPrice);
            returnDto.Category=new CategoryInProductReturnDto() { Name = product.Name ,
                ProductCount=product.Category.Products.Count()};
            return Ok(returnDto);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDTO productCreateDTO)
        {
            if(!await _shopAppContext.categories.AnyAsync(s => !s.IsDeleted && s.Id == productCreateDTO.CategoryId))
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }
            Product product = new Product();
            product.Name = productCreateDTO.Name;
            product.SalePrice= productCreateDTO.SalePrice;
            product.CostPrice= productCreateDTO.CostPrice;
            product.CategoryId = productCreateDTO.CategoryId;
            await _shopAppContext.Products.AddAsync(product);
            await _shopAppContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int? id, ProductUpdateDto product)
        {
            if (id == null) return BadRequest();
            var existedProduct = await _shopAppContext.Products.Where(s=>!s.IsDeleted)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (existedProduct == null) return NotFound();
            if (!await _shopAppContext.categories.AnyAsync(s => !s.IsDeleted && s.Id == product.CategoryId))
                return StatusCode(StatusCodes.Status409Conflict);

            existedProduct.Name = product.Name;
            existedProduct.SalePrice = product.SalePrice;
            existedProduct.CostPrice = product.CostPrice;
            existedProduct.CategoryId = product.CategoryId;
            await _shopAppContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int? id, bool status)
        {
            if (id == null) return BadRequest();
            var existedProduct = await _shopAppContext.Products.Where(s => !s.IsDeleted).FirstOrDefaultAsync(p => p.Id == id);
            if (existedProduct == null) return NotFound();
            existedProduct.IsDeleted = status;
           
            await _shopAppContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent, existedProduct);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var existedProduct = await _shopAppContext.Products.Where(s => !s.IsDeleted).FirstOrDefaultAsync(p => p.Id == id);
            if (existedProduct == null) return NotFound();
            _shopAppContext.Products.Remove(existedProduct);
            await _shopAppContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }

    }
}
