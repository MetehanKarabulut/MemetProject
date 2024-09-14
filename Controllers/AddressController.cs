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
    public class AddressController:ControllerBase
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IUserRepository _userRepository;
        public AddressController(IAddressRepository addressRepository, IUserRepository userRepository)
        {
            _addressRepository = addressRepository;  
            _userRepository = userRepository;
        }

        [HttpGet("user/address/{userId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<Address>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult getAddressesOfAUser(int userId){
            if(!ModelState.IsValid)
                return BadRequest();

            var addresses = _addressRepository.GetAddressesOfAUser(userId);

            if (addresses == null || !addresses.Any())
            {
                return NotFound();
            }

            return Ok(addresses);
        }

        [HttpGet("/address/{addressId}")]
        [ProducesResponseType(200, Type = typeof(Address))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult getAddressesById(int addressId){
            if(!ModelState.IsValid)
                return BadRequest();

            var address = _addressRepository.GetAddressById(addressId);

            if (address == null)
            {
                return NotFound();
            }

            return Ok(address);
        }

        [HttpPost("/address")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult createAddress([FromBody]Address address, [FromQuery] int UserId){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (address == null)
                return NotFound();

            if(_addressRepository.AddressExists(UserId, address.AddressTitle))
                return Conflict();

            if(!_addressRepository.CreateAddress(UserId, address))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return CreatedAtAction("GetCategoryById", new { addressId = address.AddressId}, address);;
        }
        
        [HttpDelete("/{addressId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult deleteAddress(int addressId){
            if(!_addressRepository.AddressExists(addressId))
                return NotFound();

            if (!_addressRepository.AddressDelete(addressId))
            {
                ModelState.AddModelError("", "Bir hata oluştu, kullanıcı silinemedi.");
                return StatusCode(500, ModelState);
            }
            
            return NoContent();           
        }

        [HttpPut("{AddressId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)] 
        [ProducesResponseType(400)] 
        [ProducesResponseType(500)] 
        public IActionResult UpdateAddress(int AddressId, [FromBody] Address Address)
        {
            if (Address == null || AddressId != Address.AddressId)
                return BadRequest();

            if (!_addressRepository.AddressUpdate(Address))
            {
                ModelState.AddModelError("", "Bir hata oluştu, kullanıcı güncellenemedi.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}