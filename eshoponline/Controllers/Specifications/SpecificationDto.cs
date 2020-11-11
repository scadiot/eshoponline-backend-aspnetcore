using eshoponline.Models;

namespace eshoponline.Controllers.Specifications
{
    public class SpecificationDto
    {
        public int SpecificationId { get; set; }
        public string Name { get; set; }
        public SpecificationType Type { get; set; }
        public string Unity { get; set; }
        public string LongName { get; set; }
        public int SpecificationGroupId { get; set; }
    }
}
