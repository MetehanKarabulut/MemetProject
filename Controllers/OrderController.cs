using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemetProject.Interfaces;
using MemetProject.Models;
using MemetProject.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MemetProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        
        public OrderController(IOrderRepository orderRepository, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Order>))]
        [ProducesResponseType(400)]
        public IActionResult getAllOrders(){
            if(!ModelState.IsValid)
                return BadRequest();

            var orders = _orderRepository.GetAllOrders();

            if(orders == null || !orders.Any())
                return NotFound();

            return Ok(orders);
        }

        [HttpGet("order/{orderId}")]
        [ProducesResponseType(200, Type = typeof(Order))]
        [ProducesResponseType(400)]
        public IActionResult getOrderById(int orderId){
            if(!ModelState.IsValid)
                return BadRequest();

            if(_orderRepository.OrderExists(orderId))
                return NotFound();

            var order = _orderRepository.GetOrderById(orderId);

            if(order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpGet("/order/user/{userId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<Order>))]
        [ProducesResponseType(400)]
        public IActionResult getOrdersOfAUser(int userId){
            if(!ModelState.IsValid)
                return BadRequest();

            if(_orderRepository.OrderExists(userId))
                return NotFound();

            var ordersOfAUser = _orderRepository.GetOrdersOfAUser(userId);
            
            if(ordersOfAUser == null || !ordersOfAUser.Any())
                return NotFound();
            
            return Ok(ordersOfAUser);
        }

        [HttpPost("/order")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult createorder([FromBody] Order order, [FromQuery] int userId){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _userRepository.GetUserById(userId);

            if(order == null || user == null)
                return BadRequest();            

            order.User = user;

            if(!_orderRepository.CreateOrder(order)){
                ModelState.AddModelError("", "Something went wrong");
                return  StatusCode(500, ModelState);
            }
            
            
            return CreatedAtAction("createorder", new { orderId = order.OrderId }, order);
        }

        [HttpDelete("/{orderId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult deleteOrder(int orderId){
            if(!_orderRepository.OrderExists(orderId))
                return NotFound();

            if (!_orderRepository.OrderDelete(orderId))
            {
                ModelState.AddModelError("", "Bir hata oluştu, kullanıcı silinemedi.");
                return StatusCode(500, ModelState);
            }
            
            return NoContent();           
        }

        [HttpPut("{OrderId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)] 
        [ProducesResponseType(400)] 
        [ProducesResponseType(500)] 
        public IActionResult UpdateOrder(int OrderId, [FromBody] Order Order)
        {
            if (Order == null || OrderId != Order.OrderId)
                return BadRequest();

            if (!_orderRepository.OrderUpdate(Order))
            {
                ModelState.AddModelError("", "Bir hata oluştu, kullanıcı güncellenemedi.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        
    }
}