using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Entities
{
    public class WeeklyLogs : BaseEntity
    {
        public string? GroupId { get; set; }
        public string? WeekNo { get; set; }

        public string? RoomNo { get; set; }

        public string? Summary { get; set; }

        public string? Activities { get; set; }

        public string? Result { get; set; }

        public string? Tasks { get; set; }
        public string? Status { get; set; }

        public DateTime AssignDate { get; set; }
        public DateTime UserSubmissionDate { get; set; }
        public DateTime SubmitDate { get; set; }
    }
}
