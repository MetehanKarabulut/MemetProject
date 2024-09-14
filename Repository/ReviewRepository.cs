using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemetProject.Data;
using MemetProject.Interfaces;
using MemetProject.Models;

namespace MemetProject.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;
        public ReviewRepository(DataContext context)
        {
            _context = context;
        }
        public bool CreateReview(Review review)
        {
            _context.Add(review);
            return Save();
        }

        public ICollection<Review> GetAllReviews()
        {
            return _context.Reviews.OrderBy(r => r.ReviewId).ToList();
        }

        public Review GetReviewById(int reviewId)
        {
            return _context.Reviews.Where(r => r.ReviewId == reviewId).SingleOrDefault();
        }

        public bool ReviewDelete(int reviewId)
        {
            var review = _context.Reviews.Find(reviewId);

            if(review == null)
                return false;

            _context.Reviews.Remove(review);

            return Save();
        }

        public bool ReviewExists(int reviewId)
        {
            return _context.Reviews.Any(r => r.ReviewId == reviewId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool ReviewUpdate(Review Review)
        {
            var existingReview = _context.Reviews.FirstOrDefault(u => u.ReviewId == Review.ReviewId);

            if (!ReviewExists(Review.ReviewId))
                return false;


            _context.Entry(existingReview).CurrentValues.SetValues(Review);

            _context.Reviews.Update(existingReview);
            return Save();
        }
    }
}