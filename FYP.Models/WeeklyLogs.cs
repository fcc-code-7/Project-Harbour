using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Entities
{
    public class WeeklyLogs : BaseEntity
    {
        public string? GroupName { get; set; }
        public string? RoomNo { get; set; }

        public string? Summary { get; set; }

        public string? Activities { get; set; }

        public string? Result { get; set; }

        public string? Tasks { get; set; }

        public DateTime Date { get; set; }
    }
}
