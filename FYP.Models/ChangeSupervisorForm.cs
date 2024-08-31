using FYP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Models
{
    public class ChangeSupervisorForm : BaseEntity
    {
        public string? NewSupervsiorId { get; set; }
        public string? oldsupervisorId { get; set; }
        public string? GroupId { get; set; }
        public string? Reason { get; set; }
        public string? OtherReason { get; set; }
        public DateTime CurrentDate { get; set; }
    }
}
