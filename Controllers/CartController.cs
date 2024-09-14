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
    public class CartController:ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;

        public CartController(ICartRepository cartRepository, IUserRepository userRepository)
        {
            _cartRepository = cartRepository;
            _userRepository = userRepository;
        }

        [HttpGet("/cart")]
        [ProducesResponseType(200, Type = typeof(ICollection<Cart>))]
        [ProducesResponseType(400)]
        public IActionResult getCarts(){
            var carts = _cartRepository.GetCarts();
            
            if(carts == null || !carts.Any())
                return NotFound();
            
            return Ok(carts);
        }

        [HttpGet("cart/{cartId}")]
        [ProducesResponseType(200, Type = typeof(Cart))]
        [ProducesResponseType(400)]
        public IActionResult getCartById(int cartId){
            if(!ModelState.IsValid)
                return BadRequest();

            if(_cartRepository.CartExists(cartId))
                return NotFound();

            var cart = _cartRepository.GetCartById(cartId);

            if(cart == null)
                return NotFound();

            return Ok(cart);
        }

        [HttpGet("/cart/user/{userId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<Cart>))]
        [ProducesResponseType(400)]
        public IActionResult getCartsOfAUser(int userId){
            if(!ModelState.IsValid)
                return BadRequest();

            if(!_userRepository.UserExists(userId))
                return NotFound();

            var cartsOfAUser = _cartRepository.GetCartsOfAUser(userId);
            
            if(cartsOfAUser == null || !cartsOfAUser.Any())
                return NotFound();
            
            return Ok(cartsOfAUser);
        }

        [HttpPost("/cart")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)] 
        [ProducesResponseType(404)] 
        [ProducesResponseType(500)] 
        public IActionResult CreateCart([FromBody] Cart cart)
        {
            try
            {
                Cart newCart = new Cart(){
                User = cart.User,
                CreatedDate = cart.CreatedDate,
                CartItems = cart.CartItems
            };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Cart nesnesi olusturulamadi",
                    error = ex.Message
                });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Geçersiz veri girişi.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            if (cart == null)
            {
                return NotFound(new
                {
                    message = "Cart verisi bulunamadı."
                });
            }

            try
            {
                
                if (!_cartRepository.CreateCart(cart))
                {
                    System.Console.WriteLine("girdi");
                    ModelState.AddModelError("", "Cart oluşturulurken bir hata oluştu.");
                    return StatusCode(500, new
                    {
                        message = "Sunucu hatası, cart oluşturulamadı."
                    });
                }


                return CreatedAtAction("CreateCart", new { cartId = cart.CartId }, cart);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new
                {
                    message = "Sunucuda beklenmeyen bir hata meydana geldi.",
                    error = ex.Message
                });
            }
        }


        [HttpPost("/cart/cartItem")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult addCartItemToCart([FromBody]CartItem cartItem,  [FromQuery] int cartId){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(cartItem == null || _cartRepository.CartExists(cartId) == null)
                return BadRequest();            
            
            if(!_cartRepository.AddCartItemToCart(cartItem, cartId)){
                ModelState.AddModelError("", "Something went wrong");
                return  StatusCode(500, ModelState);
            }
            
            
            return CreatedAtAction("addcartitemtocart", new { cartItemId = cartItem.CartItemId}, cartItem);
        }

        [HttpDelete("/{cartId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult deleteCart(int cartId){
            if(!_cartRepository.CartExists(cartId))
                return NotFound();

            if (!_cartRepository.CartDelete(cartId))
            {
                ModelState.AddModelError("", "Bir hata oluştu, kullanıcı silinemedi.");
                return StatusCode(500, ModelState);
            }
            
            return NoContent();           
        }

        [HttpPut("{CartId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)] 
        [ProducesResponseType(400)] 
        [ProducesResponseType(500)] 
        public IActionResult UpdateCart(int CartId, [FromBody] Cart Cart)
        {
            if (Cart == null || CartId != Cart.CartId)
                return BadRequest();

            if (!_cartRepository.CartUpdate(Cart))
            {
                ModelState.AddModelError("", "Bir hata oluştu, kullanıcı güncellenemedi.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}