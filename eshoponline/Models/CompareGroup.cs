using System.ComponentModel.DataAnnotations;

namespace eshoponline.Models
{
    public class CompareGroup
    {
        public int CompareGroupId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
