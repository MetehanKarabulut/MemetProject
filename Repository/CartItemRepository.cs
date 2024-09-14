using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemetProject.Data;
using MemetProject.Interfaces;
using MemetProject.Models;

namespace MemetProject.Repository
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly DataContext _context;
        public CartItemRepository(DataContext context)
        {
            _context = context;
        }
        public bool CartItemExists(int cartItemId)
        {
            return _context.CartItems.Any(c => c.CartItemId == cartItemId);
        }

        public bool CreateCartItem(CartItem cartItem)
        {
            _context.Add(cartItem);
            return Save();
        }

        public CartItem GetCartItemById(int cartItemId)
        {
            return _context.CartItems.Where(c => c.CartItemId == cartItemId).SingleOrDefault();
        }

        public ICollection<CartItem> GetCartItemsOfACart(int cartId)
        {
            return _context.Carts.Where(c => c.CartId == cartId).SelectMany(c => c.CartItems).ToList();
        }
        public bool CartItemDelete(int CartItemId)
        {
            var CartItem = _context.CartItems.Where(o => o.CartItemId == CartItemId).SingleOrDefault();

            if(CartItem == null)
                return false;

            _context.CartItems.Remove(CartItem);

            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
        public bool CartItemUpdate(CartItem CartItem)
        {
            var existingCartItem = _context.CartItems.FirstOrDefault(u => u.CartItemId == CartItem.CartItemId);

            if (!CartItemExists(CartItem.CartItemId))
                return false;


            existingCartItem.product = CartItem.product;
            existingCartItem.Quantity = CartItem.Quantity;

            _context.CartItems.Update(existingCartItem);
            return Save();
        }
    }
}