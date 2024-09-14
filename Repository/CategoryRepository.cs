using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemetProject.Data;
using MemetProject.Interfaces;
using MemetProject.Models;
using Microsoft.EntityFrameworkCore;

namespace MemetProject.Repository
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly DataContext _context;
        public CategoryRepository(DataContext context)
        {
            _context = context;
        }
        public bool CategoryExists(int categoryId)
        {
            return _context.Categories.Any(c => c.CategoryId == categoryId);
        }

        public bool CreateCategory(Category category)
        {
            _context.Add(category);
            return Save();
        }

        public ICollection<Category> GetAllCategories()
        {
            return _context.Categories.OrderBy(c => c.CategoryId).ToList();
        }

        public Category GetCategoryById(int categoryId)
        {
            return _context.Categories.Where(c => c.CategoryId == categoryId).SingleOrDefault();
        }

        public ICollection<Product> GetProductsOfACategory(int categoryId)
        {
            return _context.Categories.Where(c => c.CategoryId == categoryId).Select(c => c.Products).SelectMany(p => p).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool CategoryDelete(int categoryId)
        {
            var products = _context.Products.Where(c => c.Category.CategoryId == categoryId).ToList();
            _context.Products.RemoveRange(products);

            var category = _context.Categories.Where(o => o.CategoryId == categoryId).SingleOrDefault();

            if(category == null)
                return false;

            _context.Categories.Remove(category);

            return Save();
        }

        public bool CategoryUpdate(Category Category)
        {
            var existingCategory = _context.Categories.Where(u => u.CategoryId == Category.CategoryId).FirstOrDefault();

            if (!CategoryExists(Category.CategoryId))
                return false;


            existingCategory.CategoryName = Category.CategoryName;
            existingCategory.Description = Category.Description;

            _context.Categories.Update(existingCategory);
            return Save();
        }
    }
}