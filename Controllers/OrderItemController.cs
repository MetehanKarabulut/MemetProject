using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemetProject.Interfaces;
using MemetProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace MemetProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController:ControllerBase
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderItemController(IOrderItemRepository orderItemRepository, IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        [HttpGet("order-item/{orderItemId}")]
        [ProducesResponseType(200, Type = typeof(OrderItem))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult getorderItemById(int orderItemId){
            if(!ModelState.IsValid)
                return BadRequest();

            if(_orderItemRepository.OrderItemExists(orderItemId))
                return NotFound();

            var orderItem = _orderItemRepository.GetOrderItemById(orderItemId);

            if(orderItem == null){
                ModelState.AddModelError("", "Something went wrong while creating the orderItem");
                return StatusCode(500, ModelState);
            }
        
            return Ok(orderItem);
        }
        
        [HttpGet("/order/orderItem/{orderId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<OrderItem>))]
        [ProducesResponseType(400)]
        public IActionResult getOrderItemsOfAOrder(int orderId){
            if(!ModelState.IsValid)
                return BadRequest();
            
            if(_orderItemRepository.OrderItemExists(orderId))
                return NotFound();

            var orderItemsOfAOrder = _orderItemRepository.GetOrderItemsOfAOrder(orderId);
            
            if(orderItemsOfAOrder == null || !orderItemsOfAOrder.Any())
                return NotFound();
            
            return Ok(orderItemsOfAOrder);
        } 

        [HttpPost("/orderItem")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult createOrderItem([FromBody] OrderItem orderItem, [FromQuery] int orderId, [FromQuery] int productId){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = _productRepository.GetProductById(productId);
            var order = _orderRepository.GetOrderById(orderId);

            if(orderItem == null || product == null || order == null)
                return BadRequest();

            orderItem.Order = order;
            orderItem.Product = product;

            if(!_orderItemRepository.CreateOrderItem(orderItem)){
                ModelState.AddModelError("", "Something went wrong");
                return  StatusCode(500, ModelState);
            }
            
            
            return CreatedAtAction("createorderItem", new { orderItemId = orderItem.OrderItemId }, orderItem);
        }

        [HttpDelete("/{orderItemId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult deleteOrderItem(int orderItemId){
            if(!_orderItemRepository.OrderItemExists(orderItemId))
                return NotFound();

            if (!_orderItemRepository.OrderItemDelete(orderItemId))
            {
                ModelState.AddModelError("", "Bir hata oluştu, kullanıcı silinemedi.");
                return StatusCode(500, ModelState);
            }
            
            return NoContent();           
        }

        [HttpPut("{OrderItemId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)] 
        [ProducesResponseType(400)] 
        [ProducesResponseType(500)] 
        public IActionResult UpdateOrderItem(int OrderItemId, [FromBody] OrderItem OrderItem)
        {
            if (OrderItem == null || OrderItemId != OrderItem.OrderItemId)
                return BadRequest();

            if (!_orderItemRepository.OrderItemUpdate(OrderItem))
            {
                ModelState.AddModelError("", "Bir hata oluştu, kullanıcı güncellenemedi.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}