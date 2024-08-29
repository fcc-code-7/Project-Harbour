using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Entities
{
    public class ChangeTitleForm : BaseEntity
    {
        public  string? oldPID { get; set; }
        public string?Status { get; set; }
        public bool SupervisorStatus { get; set; }
        public string? CommiteeID { get; set; }

    }
}
