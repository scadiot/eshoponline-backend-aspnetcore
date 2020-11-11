using System.ComponentModel.DataAnnotations;

namespace eshoponline.Models
{
    public class ProductFeature
    {
        public int ProductFeatureId { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
