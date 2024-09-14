using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemetProject.Models;

namespace MemetProject.Interfaces
{
    public interface IProductRepository
    {
        bool ProductExists(int productId);
        bool Save();
        bool ProductDelete(int productId);
        bool CreateProduct(Product product);
        ICollection<Product> GetAllProducts();
        ICollection<Review> GetProductReviews(int productId);
        Product GetProductById(int productId);
        bool ProductUpdate(Product product);
    }
}