using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Entities
{
    public class Company : BaseEntity
    {
        public string? Name { get; set; }
        public string? MentorName { get; set; }
        public string? MentorTelephone { get; set; }
        public string? MentorEmail { get; set; }
    }
}
