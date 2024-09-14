using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using MemetProject.Interfaces;
using MemetProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace MemetProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController:ControllerBase
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductRepository _productRepository;
        public CartItemController(ICartItemRepository cartItemRepository, IProductRepository productRepository)
        {
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
        }

        [HttpGet("cart-item/{cartItemId}")]
        [ProducesResponseType(200, Type = typeof(CartItem))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult getCartItemById(int cartItemId){
            if(!ModelState.IsValid)
                return BadRequest();

            if(!_cartItemRepository.CartItemExists(cartItemId))
                return NotFound();

            var cartItem = _cartItemRepository.GetCartItemById(cartItemId);

            if(cartItem == null){
                ModelState.AddModelError("", "Something went wrong while creating the cartItem");
                return StatusCode(500, ModelState);
            }
        
            return Ok(cartItem);
        }

        [HttpPost("/cartItem")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult createCartItem([FromBody] CartItem cartItem, [FromQuery] int productId, [FromQuery] int quantity){
            cartItem.product = _productRepository.GetProductById(productId);
            cartItem.Quantity = quantity;
            
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(cartItem == null)
                return BadRequest();
            
            if(!_cartItemRepository.CreateCartItem(cartItem)){
                ModelState.AddModelError("", "Something went wrong");
                return  StatusCode(500, ModelState);
            }
            
            
            return CreatedAtAction("createCartItem", new { cartItemId = cartItem.CartItemId }, cartItem);
        }

        [HttpGet("/cart/cartItem/{cartId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<CartItem>))]
        [ProducesResponseType(400)]
        public IActionResult getCartItemsOfACart(int cartId){
            if(!ModelState.IsValid)
                return BadRequest();
            
            if(_cartItemRepository.CartItemExists(cartId))
                return NotFound();

            var cartItemsOfACart = _cartItemRepository.GetCartItemsOfACart(cartId);
            
            if(cartItemsOfACart == null || !cartItemsOfACart.Any())
                return NotFound();
            
            return Ok(cartItemsOfACart);
        }     
        
        [HttpDelete("/{cartItemId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult deleteCartItem(int cartItemId){
            if(!_cartItemRepository.CartItemExists(cartItemId))
                return NotFound();

            if (!_cartItemRepository.CartItemDelete(cartItemId))
            {
                ModelState.AddModelError("", "Bir hata oluştu, kullanıcı silinemedi.");
                return StatusCode(500, ModelState);
            }
            
            return NoContent();           
        }

        [HttpPut("{CartItemId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)] 
        [ProducesResponseType(400)] 
        [ProducesResponseType(500)] 
        public IActionResult UpdateCartItem([FromBody] CartItem CartItem)
        {
            if (CartItem == null)
                return BadRequest();

            if (!_cartItemRepository.CartItemUpdate(CartItem))
            {
                ModelState.AddModelError("", "Bir hata oluştu, kullanıcı güncellenemedi.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
           
    }
}