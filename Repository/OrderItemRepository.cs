using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemetProject.Data;
using MemetProject.Interfaces;
using MemetProject.Models;

namespace MemetProject.Repository
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly DataContext _context;
        public OrderItemRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateOrderItem(OrderItem orderItem)
        {
            _context.Add(orderItem);
            return Save();
        }

        public ICollection<OrderItem> GetAllOrderItems()
        {
            return _context.OrderItems.OrderBy(o => o.OrderItemId).ToList();
        }

        public OrderItem GetOrderItemById(int orderItemId)
        {
            return _context.OrderItems.Where(o => o.OrderItemId == orderItemId).SingleOrDefault();
        }

        public ICollection<OrderItem> GetOrderItemsOfAOrder(int OrderId)
        {
            return _context.Orders.Where(o => o.OrderId == OrderId).SelectMany(o => o.OrderItems).ToList();
        }

        public bool OrderItemExists(int orderItemId)
        {
            return _context.OrderItems.Any(o => o.OrderItemId == orderItemId);
        }

        public bool OrderItemDelete(int orderItemId)
        {
            var orderItem = _context.OrderItems.Where(o => o.OrderItemId == orderItemId).SingleOrDefault();

            if(orderItem == null)
                return false;

            _context.OrderItems.Remove(orderItem);

            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
        public bool OrderItemUpdate(OrderItem OrderItem)
        {
            var existingOrderItem = _context.OrderItems.FirstOrDefault(u => u.OrderItemId == OrderItem.OrderItemId);

            if (!OrderItemExists(OrderItem.OrderItemId))
                return false;


            _context.Entry(existingOrderItem).CurrentValues.SetValues(OrderItem);

            _context.OrderItems.Update(existingOrderItem);
            return Save();
        }
    }
}