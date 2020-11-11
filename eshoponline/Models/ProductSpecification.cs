using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshoponline.Models
{
    public class ProductSpecification
    {
        public int Id { get; set; }

        public int IntegerValue { get; set; }

        public decimal DecimalValue { get; set; }

        public bool BooleanValue { get; set; }

        public string StringValue { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int SpecificationId { get; set; }

        public virtual Specification Specification { get; set; }
    }
}
