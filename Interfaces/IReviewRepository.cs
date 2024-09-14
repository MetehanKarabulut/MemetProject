using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemetProject.Models;

namespace MemetProject.Interfaces
{
    public interface IReviewRepository
    {
        bool ReviewExists(int reviewId);
        bool Save();
        bool ReviewDelete(int ReviewId);
        bool CreateReview(Review review);
        Review GetReviewById(int reviewId);
        ICollection<Review> GetAllReviews();
        bool ReviewUpdate(Review review);
    }
}