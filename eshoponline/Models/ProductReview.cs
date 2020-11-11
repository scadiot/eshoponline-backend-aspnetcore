using System;
using System.ComponentModel.DataAnnotations;

namespace eshoponline.Models
{
    public class ProductReview
    {
        public int ProductReviewId { get; set; }

        public string Title { get; set; }

        [Required]
        public string Comment { get; set; }

        public DateTime Date { get; set; }

        public double Stars { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
