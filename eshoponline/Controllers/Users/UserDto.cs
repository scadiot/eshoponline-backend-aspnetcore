using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshoponline.Controllers.Users
{
    public class UserDto
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public string Pseudonym { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        // n-1 relationships
        public int AddressId { get; set; }
    }
}
