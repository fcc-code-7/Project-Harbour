using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Entities
{
    public class RoomAllotment  : BaseEntity
    {
        public string? InchargeId { get; set; }
        public string? Room { get; set; }
        public string? Batch { get; set; }
        public string? Evaluation { get; set; }
    }
}
