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
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository _userRepository;

        public ReviewController(IReviewRepository reviewRepository, IUserRepository userRepository)
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Review>))]
        [ProducesResponseType(400)]
        public IActionResult getAllReviews(){
            if(!ModelState.IsValid)
                return BadRequest();

            var reviews = _reviewRepository.GetAllReviews();

            if(reviews == null || !reviews.Any())
                return NotFound();

            return Ok(reviews);
        }

        [HttpGet("/review/{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult getReviewById(int reviewId){
            if(!ModelState.IsValid)
                return BadRequest();

            if(_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            var review = _reviewRepository.GetReviewById(reviewId);

            if(review == null){
                ModelState.AddModelError("", "Something went wrong while creating the review");
                return StatusCode(500, ModelState);
            }
        
            return Ok(review);
        }

        [HttpPost("/review")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult createreview([FromBody] Review review, int UserId){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(review == null)
                return BadRequest();
            
            var user = _userRepository.GetUserById(UserId);
            review.User = user;

            if(!_reviewRepository.CreateReview(review)){
                ModelState.AddModelError("", "Something went wrong");
                return  StatusCode(500, ModelState);
            }
            
            
            return CreatedAtAction("createreview", new { reviewId = review.ReviewId }, review);
        }

        [HttpDelete("/{reviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult deleteReview(int reviewId){
            if(!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            if (!_reviewRepository.ReviewDelete(reviewId))
            {
                ModelState.AddModelError("", "Bir hata oluştu, gonderi silinemedi.");
                return StatusCode(500, ModelState);
            }
            
            return NoContent();           
        }

        [HttpPut("{ReviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)] 
        [ProducesResponseType(400)] 
        [ProducesResponseType(500)] 
        public IActionResult updateReview(int ReviewId, [FromBody] Review Review)
        {
            if (Review == null || ReviewId != Review.ReviewId)
                return BadRequest();

            if (!_reviewRepository.ReviewUpdate(Review))
            {
                ModelState.AddModelError("", "Bir hata oluştu, kullanıcı güncellenemedi.");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}