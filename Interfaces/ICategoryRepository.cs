using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemetProject.Models;

namespace MemetProject.Interfaces
{
    public interface ICategoryRepository
    {
        bool CategoryExists(int categoryId);
        bool Save();
        bool CategoryDelete(int categoryId);
        bool CreateCategory(Category category);
        ICollection<Category> GetAllCategories();
        Category GetCategoryById(int categoryId);
        ICollection<Product> GetProductsOfACategory(int categoryId);
        bool CategoryUpdate(Category category);
           
    }
}