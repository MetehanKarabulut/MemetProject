using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemetProject.Data;
using MemetProject.Interfaces;
using MemetProject.Models;

namespace MemetProject.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly DataContext _context;
        public CartRepository(DataContext context)
        {
            _context = context;
        }
        public bool AddCartItemToCart(CartItem cartItem, int cartId)
        {
            _context.Carts.Where(c => c.CartId == cartId).SingleOrDefault().CartItems.Add(cartItem);

            return Save();
        }

        public bool CartExists(int cartId)
        {
            return _context.Carts.Any(u => u.CartId == cartId);
        }

        public bool CreateCart(Cart cart)
        {
            try{
                    _context.Add(cart);
                    return Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
                throw;
            }

        }

        public Cart GetCartById(int cartId)
        {
            return _context.Carts.Where(u => u.CartId == cartId).SingleOrDefault();
        }

        public ICollection<Cart> GetCartsOfAUser(int UserId)
        {
            return _context.Carts.Where(a => a.User.UserId == UserId).ToList();
        }

        public ICollection<Cart> GetCarts(){
            return _context.Carts.OrderBy(c => c.CartId).ToList();
        }

        public bool CartDelete(int CartId)
        {
            var Cart = _context.Carts.Where(o => o.CartId == CartId).SingleOrDefault();

            if(Cart == null)
                return false;

            _context.Carts.Remove(Cart);

            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool CartUpdate(Cart Cart)
        {
            var existingCart = _context.Carts.FirstOrDefault(u => u.CartId == Cart.CartId);

            if (!CartExists(Cart.CartId))
                return false;


            _context.Entry(existingCart).CurrentValues.SetValues(Cart);

            _context.Carts.Update(existingCart);
            return Save();
        }
    }
}