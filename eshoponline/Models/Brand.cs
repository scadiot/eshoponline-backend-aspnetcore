using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eshoponline.Models
{
    public class Brand
    {
        public int BrandId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Slug { get; set; }

        public int ProductsCount { get; set; }
    }
}
