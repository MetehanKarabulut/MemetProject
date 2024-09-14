using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemetProject.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public User? User { get; set; }
        public Product? Product { get; set; }
        public string? Comment { get; set; }
        public required int Rating { get; set; }

    }
}