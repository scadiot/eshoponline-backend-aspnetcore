using eshoponline.Models;
using System;

namespace eshoponline.Controllers.ProductReviews
{
    public class ProductReviewDto
    {
        public int ProductReviewId { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public double Stars { get; set; }
        public int UserId { get; set; }
        public string UserPseudonym { get; set; }
        public int ProductId { get; set; }
    }
}
