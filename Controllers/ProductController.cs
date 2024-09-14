using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemetProject.Interfaces;
using MemetProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace MemetProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController:ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Product>))]
        [ProducesResponseType(400)]
        public IActionResult getAllProducts(){
            if(!ModelState.IsValid)
                return BadRequest();

            var products = _productRepository.GetAllProducts();

            if(products == null || !products.Any())
                return NotFound();

            return Ok(products);
        }

        [HttpGet("/product/{productId}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult getProductById(int productId){
            if(!ModelState.IsValid)
                return BadRequest();

            if(_productRepository.ProductExists(productId))
                return NotFound();

            var product = _productRepository.GetProductById(productId);

            if(product == null){
                ModelState.AddModelError("", "Something went wrong while creating the product");
                return StatusCode(500, ModelState);
            }
        
            return Ok(product);
        }

        [HttpPost("/product")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult createproduct([FromBody] Product product, [FromQuery] int categoryId){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(product == null || !_categoryRepository.CategoryExists(categoryId))
                return NotFound();
            
            var category = _categoryRepository.GetCategoryById(categoryId);

            product.Category = category;

            if(!_productRepository.CreateProduct(product)){
                ModelState.AddModelError("", "Something went wrong");
                return  StatusCode(500, ModelState);
            }
                        
            return CreatedAtAction("createproduct", new { productId = product.ProductId }, product);;
        }

        [HttpGet("/product/reviews/{productId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<Review>))]
        [ProducesResponseType(400)]
        public IActionResult getProductReviews(int productId){
            if(!ModelState.IsValid)
                return BadRequest();
            
            if(_productRepository.ProductExists(productId))
                return NotFound();

            var reviews = _productRepository.GetProductReviews(productId);
            
            if(reviews == null || !reviews.Any())
                return NotFound();
            
            return Ok(reviews);
        }

        [HttpDelete("Product/{productId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult deleteProduct(int productId){
            if(!_productRepository.ProductExists(productId))
                return NotFound();

            if (!_productRepository.ProductDelete(productId))
            {
                ModelState.AddModelError("", "Bir hata oluştu, kullanıcı silinemedi.");
                return StatusCode(500, ModelState);
            }
            
            return NoContent();           
        }

        [HttpPut("{ProductId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)] 
        [ProducesResponseType(400)] 
        [ProducesResponseType(500)] 
        public IActionResult UpdateProduct(int ProductId, [FromBody] Product Product)
        {
            if (Product == null || ProductId != Product.ProductId)
                return BadRequest();

            if (!_productRepository.ProductUpdate(Product))
            {
                ModelState.AddModelError("", "Bir hata oluştu, kullanıcı güncellenemedi.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}