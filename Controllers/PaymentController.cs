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
    public class PaymentController:ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;

        public PaymentController(IPaymentRepository paymentRepository, IOrderRepository orderRepository)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Payment>))]
        [ProducesResponseType(400)]
        public IActionResult getAllPayments(){
            if(!ModelState.IsValid)
                return BadRequest();

            var payments = _paymentRepository.GetAllPayments();

            if(payments == null || !payments.Any())
                return NotFound();

            return Ok(payments);
        }

        [HttpGet("/payment/{paymentId}")]
        [ProducesResponseType(200, Type = typeof(Payment))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult getpaymentById(int paymentId){
            if(!ModelState.IsValid)
                return BadRequest();

            if(_paymentRepository.PaymentExists(paymentId))
                return NotFound();

            var payment = _paymentRepository.GetPaymentById(paymentId);

            if(payment == null){
                ModelState.AddModelError("", "Something went wrong while creating the payment");
                return StatusCode(500, ModelState);
            }
        
            return Ok(payment);
        }

        [HttpPost("/payment")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult createPayment([FromBody] Payment payment, [FromQuery] int orderId){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(payment == null)
                return BadRequest();
            
            payment.OrderId = orderId;
            payment.Order = _orderRepository.GetOrderById(orderId);

            if(!_paymentRepository.CreatePayment(payment)){
                ModelState.AddModelError("", "Something went wrong");
                return  StatusCode(500, ModelState);
            }
            
            
            return CreatedAtAction("createpayment", new { paymentId = payment.PaymentId }, payment);
        }

        [HttpDelete("/{paymentId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult deletePayment(int paymentId){
            if(!_paymentRepository.PaymentExists(paymentId))
                return NotFound();

            if (!_paymentRepository.PaymentDelete(paymentId))
            {
                ModelState.AddModelError("", "Bir hata oluştu, kullanıcı silinemedi.");
                return StatusCode(500, ModelState);
            }
            
            return NoContent();           
        }
        [HttpPut("{PaymentId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)] 
        [ProducesResponseType(400)] 
        [ProducesResponseType(500)] 
        public IActionResult UpdatePayment(int PaymentId, [FromBody] Payment Payment)
        {
            if (Payment == null || PaymentId != Payment.PaymentId)
                return BadRequest();

            if (!_paymentRepository.PaymentUpdate(Payment))
            {
                ModelState.AddModelError("", "Bir hata oluştu, kullanıcı güncellenemedi.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}