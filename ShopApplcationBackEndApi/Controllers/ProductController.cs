using Microsoft.AspNetCore.Mvc;
using ShopApplcationBackEndApi.Entities;
using System.Collections.Generic;

namespace ShopApplicationBackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public List<Product> Products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Iphone11",
                Price = 200
            },
            new Product
            {
                Id = 2,
                Name = "Iphone12",
                Price = 300
            },
            new Product
            {
                Id = 3,
                Name = "Iphone13",
                Price = 400
            }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            return Ok(Products);
        }

        [HttpGet("{id}")]
        public ActionResult<Product> Get(int? id)
        {
            if(id == null) return BadRequest("id can never be null");
            var product = Products.Find(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            Products.Add(product);
            return StatusCode(StatusCodes.Status201Created,product);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int? id,Product product)
        {
            if (id == null) return BadRequest();
            var existedProduct=Products.FirstOrDefault(p => p.Id == id);
            if (existedProduct == null) return NotFound();
            existedProduct.Name = product.Name;
            existedProduct.Price = product.Price;
            existedProduct.Description = product.Description;
            existedProduct.IsDeleted = product.IsDeleted;
          
            return StatusCode(StatusCodes.Status204NoContent,existedProduct);
        }
        [HttpPatch("{id}")]
        public IActionResult Patch(int? id,bool status)
        {
            if (id == null) return BadRequest();
            var existedProduct = Products.FirstOrDefault(p => p.Id == id);
            if (existedProduct == null) return NotFound();
            existedProduct.IsDeleted= status;
            return StatusCode(StatusCodes.Status204NoContent, existedProduct);

        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int? id)
        {
            if (id == null) return BadRequest();
            var existedProduct = Products.FirstOrDefault(s => s.Id == id);
            if(existedProduct == null) return NotFound();
            Products.Remove(existedProduct);
            return StatusCode(StatusCodes.Status204NoContent);
        }

    }
}
