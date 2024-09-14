using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemetProject.Models;

namespace MemetProject.Interfaces
{
    public interface IOrderRepository
    {
        bool OrderExists(int orderId);
        bool CreateOrder(Order order);
        bool OrderDelete(int orderId);
        bool Save();
        Order GetOrderById(int orderId);
        ICollection<Order> GetAllOrders();
        ICollection<Order> GetOrdersOfAUser(int UserId);
        bool OrderUpdate(Order order);
    }
}