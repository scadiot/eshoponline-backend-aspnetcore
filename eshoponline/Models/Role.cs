using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshoponline.Models
{
    public enum RoleLabel
    {
        administrator,
        manager
    }

    public class Role
    {
        public int RoleId { get; set; }

        public RoleLabel Label { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
