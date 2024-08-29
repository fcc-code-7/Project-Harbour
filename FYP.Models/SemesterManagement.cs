using FYP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Models
{
    public class SemesterManagement : BaseEntity
    {
        public string? Name { get; set; }
        public string? Batch { get; set; }
        public string? Year { get; set; }
    }
}
