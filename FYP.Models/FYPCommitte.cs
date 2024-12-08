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
        public string? EvaluationID  { get; set; }
        public string? Member1ID { get; set; }
        public string? Member2ID { get; set; }
        public string? ExternalId { get; set; }
        public string? Room { get; set; }
        public DateTime AppointedDate{ get; set; }
        public TimeOnly AppointedTime{ get; set; }
        public TimeOnly Endime{ get; set; }
        public string? batch { get; set; }
    }
}
