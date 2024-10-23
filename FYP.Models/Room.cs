using FYP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Models
{
    public class Room : BaseEntity
    {
        public string? RoomNo { get; set; }
        public string? Department { get; set; }
    }
}
