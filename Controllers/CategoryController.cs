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
    public class CategoryController:ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Category>))]
        [ProducesResponseType(400)]
        public IActionResult getAllCategories(){
            if(!ModelState.IsValid)
                return BadRequest();

            var categories = _categoryRepository.GetAllCategories();

            if(categories == null || !categories.Any())
                return NotFound();

            return Ok(categories);
        }

        [HttpGet("/category/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult getCategoryById(int categoryId){
            if(!ModelState.IsValid)
                return BadRequest();

            if(_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var category = _categoryRepository.GetCategoryById(categoryId);

            if(category == null){
                ModelState.AddModelError("", "Something went wrong while creating the category");
                return StatusCode(500, ModelState);
            }
        
            return Ok(category);
        }

        [HttpGet("/category/product/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<Product>))]
        [ProducesResponseType(400)]
        public IActionResult getProductsOfACategory(int categoryId){
            if(!ModelState.IsValid)
                return BadRequest();
            
            if(!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var productsOfACategory = _categoryRepository.GetProductsOfACategory(categoryId);
            
            if(productsOfACategory == null || !productsOfACategory.Any())
                return NotFound();
            
            return Ok(productsOfACategory);
        }

        [HttpPost("/category")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult createCategory([FromBody] Category category){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(category == null)
                return BadRequest();
            
            if(!_categoryRepository.CreateCategory(category)){
                ModelState.AddModelError("", "Something went wrong");
                return  StatusCode(500, ModelState);
            }
            
            
            return CreatedAtAction("createCategory", new { categoryId = category.CategoryId }, category);
        }

        [HttpDelete("Category/{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult deleteCategory(int categoryId){
            if(!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            if (!_categoryRepository.CategoryDelete(categoryId))
            {
                ModelState.AddModelError("", "Bir hata oluştu, kullanıcı silinemedi.");
                return StatusCode(500, ModelState);
            }
            
            return NoContent();           
        }

        [HttpPut("{CategoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)] 
        [ProducesResponseType(400)] 
        [ProducesResponseType(500)] 
        public IActionResult UpdateCategory(int CategoryId, [FromBody] Category Category)
        {
            if (Category == null || CategoryId != Category.CategoryId)
                return BadRequest();

            if (!_categoryRepository.CategoryUpdate(Category))
            {
                ModelState.AddModelError("", "Bir hata oluştu, kullanıcı güncellenemedi.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}