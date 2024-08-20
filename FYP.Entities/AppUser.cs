using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Entities
{
    public class AppUser : BaseEntity
    {
        public string Cnic { get; set; }
        public string Department { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
