using System.ComponentModel.DataAnnotations;

namespace eshoponline.Models
{
    public class Address
    {
        public int AddressId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string Street { get; set; }

        public string Street2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
