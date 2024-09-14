using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemetProject.Data;
using MemetProject.Interfaces;
using MemetProject.Models;

namespace MemetProject.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;
        public OrderRepository(DataContext context)
        {
            _context = context;
        }
        public bool CreateOrder(Order order)
        {
            _context.Add(order);
            return Save();
        }

        public ICollection<Order> GetAllOrders()
        {
            return _context.Orders.OrderBy(o => o.OrderId).ToList();
        }

        public Order GetOrderById(int orderId)
        {
            return _context.Orders.Where(c => c.OrderId == orderId).SingleOrDefault();
        }

        public ICollection<Order> GetOrdersOfAUser(int UserId)
        {
            return _context.Users.Where(u => u.UserId == UserId).SelectMany(u => u.Orders).ToList();
        }

        public bool OrderDelete(int orderId)
        {
            var order = _context.Orders.Where(o => o.OrderId == orderId).SingleOrDefault();

            if(order == null)
                return false;

            _context.Orders.Remove(order);

            return Save();
        }

        public bool OrderExists(int orderId)
        {
            return _context.Orders.Any(c => c.OrderId == orderId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
        public bool OrderUpdate(Order Order)
        {
            var existingOrder = _context.Orders.FirstOrDefault(u => u.OrderId == Order.OrderId);

            if (!OrderExists(Order.OrderId))
                return false;


            _context.Entry(existingOrder).CurrentValues.SetValues(Order);

            _context.Orders.Update(existingOrder);
            return Save();
        }
    }
}