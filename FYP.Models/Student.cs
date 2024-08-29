using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Entities
{
    public class Student : BaseEntity
    {
        public string? studentId { get; set; }
        public int RegNo { get; set; }
        public string? ENo { get; set; }
        public string? Semester { get; set; }
    }
}
