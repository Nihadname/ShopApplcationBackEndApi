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
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            return Ok(await _shopAppContext.Products.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int? id)
        {
            if (id == null) return BadRequest("id can never be null");
            var product = await _shopAppContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDTO productCreateDTO)
        {
            Product product = new Product();
            product.Name = productCreateDTO.Name;
            product.SalePrice= productCreateDTO.SalePrice;
            product.CostPrice= productCreateDTO.CostPrice;
            await _shopAppContext.Products.AddAsync(product);
            await _shopAppContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int? id, Product product)
        {
            if (id == null) return BadRequest();
            var existedProduct = await _shopAppContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (existedProduct == null) return NotFound();
            existedProduct.Name = product.Name;
            existedProduct.SalePrice = product.SalePrice;
            existedProduct.CostPrice = product.CostPrice;
            existedProduct.IsDeleted = product.IsDeleted;
            await _shopAppContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int? id, bool status)
        {
            if (id == null) return BadRequest();
            var existedProduct = await _shopAppContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (existedProduct == null) return NotFound();
            existedProduct.IsDeleted = status;
            await _shopAppContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent, existedProduct);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var existedProduct = await _shopAppContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (existedProduct == null) return NotFound();
            _shopAppContext.Products.Remove(existedProduct);
            await _shopAppContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }

    }
}
