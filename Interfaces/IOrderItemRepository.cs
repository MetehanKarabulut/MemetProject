using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemetProject.Models;

namespace MemetProject.Interfaces
{
    public interface IOrderItemRepository
    {
        bool OrderItemExists(int orderItemId);
        bool CreateOrderItem(OrderItem orderItem);
        bool Save();
        bool OrderItemDelete(int orederItemId);
        OrderItem GetOrderItemById(int orderItemId);
        ICollection<OrderItem> GetAllOrderItems();
        ICollection<OrderItem> GetOrderItemsOfAOrder(int OrderId);
        bool OrderItemUpdate(OrderItem orderItem);
    }
}