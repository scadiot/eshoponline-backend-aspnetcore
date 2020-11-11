

using System.ComponentModel.DataAnnotations;

namespace eshoponline.Models
{
    public class Blog
    {
        public int BlogId { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string ImageFile { get; set; }
    }
}
