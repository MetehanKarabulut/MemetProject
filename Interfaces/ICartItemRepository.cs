using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemetProject.Models;

namespace MemetProject.Interfaces
{
    public interface ICartItemRepository
    {
        bool CartItemExists(int cartItemId);
        bool Save();
        bool CartItemDelete(int cartItemId);
        bool CreateCartItem(CartItem cartItem);
        CartItem GetCartItemById(int cartItemId);
        ICollection<CartItem> GetCartItemsOfACart(int cartId);
        bool CartItemUpdate(CartItem cartItem);
    }
}