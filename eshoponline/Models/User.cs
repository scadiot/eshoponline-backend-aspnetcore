using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eshoponline.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        [Required]
        public string Pseudonym { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        // n-1 relationships
        public int? AddressId { get; set; }
        public virtual Address Address { get; set; }

        // 1-n relationships
        public virtual ICollection<CartProduct> CartItems { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<ProductReview> ProductReviews { get; set; }
    }
}
