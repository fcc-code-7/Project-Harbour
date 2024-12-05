using FYP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Models
{
    public class Notifications:BaseEntity
    {
        public string? UserId { get; set; }
        public string? Batch { get; set; }
        public string? Message { get; set; }
        public string? Subject { get; set; }
        public string? SenderId { get; set; }
    }
}
