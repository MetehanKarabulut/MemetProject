using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemetProject.Models;

namespace MemetProject.Interfaces
{
    public interface ICartRepository
    {
        bool CartExists(int cartId);
        ICollection<Cart> GetCarts();
        bool Save();
        bool CartDelete(int cartId);
        bool CreateCart(Cart cart);
        Cart GetCartById(int cartId);
        ICollection<Cart> GetCartsOfAUser(int userId);
        bool AddCartItemToCart(CartItem cartItem, int cartId);
        bool CartUpdate(Cart cart);
    }
}