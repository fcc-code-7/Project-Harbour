using FYP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Models
{
    public class RoomInCharge : BaseEntity
    {
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? RoomAlloted { get; set; }
        public string? AllotedDate { get; set; }

    }
}
