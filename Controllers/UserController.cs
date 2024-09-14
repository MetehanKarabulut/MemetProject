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
    public class UserController:ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAddressRepository _addressRepository;

        public UserController(IUserRepository userRepository, IAddressRepository addressRepository)
        {
            _userRepository = userRepository;
            _addressRepository = addressRepository;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<User>))]
        public IActionResult getAllUsers(){
            if(!ModelState.IsValid)
                return BadRequest();
            
            var users = _userRepository.GetAllUsers();

            if(users == null)
                return NotFound();

            return Ok(users);
        }

        [HttpGet("{UserId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult getUserById(int UserId){
            if(!ModelState.IsValid)
                return BadRequest();

            if(!_userRepository.UserExists(UserId))
                return NotFound();
            
            var user = _userRepository.GetUserById(UserId);
            
            return Ok(user);
        }

        [HttpGet("user/address/{UserId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<Address>))]
        [ProducesResponseType(400)]
        public IActionResult getAddressesOfAUser(int UserId){
            if(!ModelState.IsValid)
                return BadRequest();
            
            if(!_userRepository.UserExists(UserId))
                return NotFound();

            var addresses = _userRepository.GetAddressesOfAUser(UserId);
            return Ok(addresses);
        }

        [HttpGet("user/orders/{UserId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<Order>))]
        [ProducesResponseType(400)]
        public IActionResult getOrdersOfAUser(int UserId){
            if(!ModelState.IsValid)
                return BadRequest();
            
            if(!_userRepository.UserExists(UserId))
                return NotFound();

            var orders = _userRepository.GetOrdersOfAUser(UserId);
            return Ok(orders);
        }

        [HttpPost("/signup")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult createUser([FromBody] User user){
            if(user is null)
                return BadRequest();

            if(_userRepository.GetAllUsers().Where(u => u.EMail == user.EMail) is null){
                ModelState.AddModelError("", "Bu mail kullaniliyor.");
                return StatusCode(422, ModelState);
            }
            
            if (user.UserAddress != null && user.UserAddress.Any())
            {
                foreach (var address in user.UserAddress)
                {
                    address.User = user;
                }
            }
            System.Console.WriteLine(user.FirstName.ToString());
           if(!ModelState.IsValid){
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        System.Console.WriteLine(error.ErrorMessage);
                    }
                }
                return BadRequest(ModelState);     
            }

            if(!_userRepository.CreateUser(user)){
                ModelState.AddModelError("", "Birseyler ters gitti");
                return StatusCode(500, ModelState);
            }

            return CreatedAtAction("createUser", new { userId = user.UserId }, user);;
        }


        [HttpPost("/login")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult checkUser([FromQuery] string eMail, [FromQuery] string password){
            if(eMail is null || password is null)
                return BadRequest();            

            var user = _userRepository.userLogin(eMail, password);

            if(user == null){
                ModelState.AddModelError("", "Birseyler ters gitti");
                return StatusCode(500, ModelState);
            }

            return CreatedAtAction("checkUser", new { userId = user.UserId }, user);;
        }

        [HttpDelete("User/{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult deleteUser(int userId){
            if(!_userRepository.UserExists(userId))
                return NotFound();

            if (!_userRepository.UserDelete(userId))
            {
                ModelState.AddModelError("", "Bir hata oluştu, kullanıcı silinemedi.");
                return StatusCode(500, ModelState);
            }
            
            return NoContent();           
        }

        [HttpPut("{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)] 
        [ProducesResponseType(400)] 
        [ProducesResponseType(500)] 
        public IActionResult updateUser(int userId, [FromBody] User user)
        {

            if (user == null)
                return BadRequest();

            if (!_userRepository.UserUpdate(user, userId))
            {
                ModelState.AddModelError("", "Bir hata oluştu, kullanıcı güncellenemedi.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}