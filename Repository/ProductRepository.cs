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
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;
        public ProductRepository(DataContext context)
        {
            _context = context;
        }
        public bool CreateProduct(Product product)
        {
            _context.Add(product);
            return Save();
        }

        public ICollection<Product> GetAllProducts()
        {
            return _context.Products.OrderBy(p => p.ProductId).ToList();
        }

        public Product GetProductById(int productId)
        {
            return _context.Products.Where(p => p.ProductId == productId).SingleOrDefault();
        }

        public ICollection<Review> GetProductReviews(int productId)
        {
            return _context.Products.Where(p => p.ProductId == productId).SelectMany(p => p.Reviews).ToList();
        }

        public bool ProductDelete(int productId)
        {
            var product = _context.Products.Where(p => p.ProductId == productId).SingleOrDefault();

            if (product == null)
                return false;

            _context.Products.Remove(product);

            return Save();
        }

        public bool ProductExists(int productId)
        {
            return _context.Products.Any(p => p.ProductId == productId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
        public bool ProductUpdate(Product Product)
        {
            var existingProduct = _context.Products.Where(u => u.ProductId == Product.ProductId).FirstOrDefault();

            if (!ProductExists(Product.ProductId))
                return false;

            existingProduct.ProductName = Product.ProductName;
            existingProduct.Description = Product.Description;
            existingProduct.Price = Product.Price;
            existingProduct.StockControl = Product.StockControl;

            _context.Products.Update(existingProduct);
            return Save();
        }
    }
}