using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Entities
{
    public class FYPCommitte : BaseEntity
    {
        public string? groupID { get; set; }
        public string? Member1ID { get; set; }
        public string? Member2ID { get; set; }
    }
}
