using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace eshoponline.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SpecificationType
    {
        Boolean,
        Interger,
        String,
        Decimal
    }

    public class Specification
    {
        public int SpecificationId { get; set; }
        public string Name { get; set; }
        public SpecificationType Type { get; set; }
        public string Unity { get; set; }
        public string LongName { get; set; }
        public int? SpecificationGroupId { get; set; }
        public virtual SpecificationGroup SpecificationGroup { get; set; }
    }
}
