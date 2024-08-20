using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Entities
{
    public class StudentGroup : BaseEntity
    {
        public string student1ID { get; set; }
        public string student2ID { get; set; }
        public string student3ID { get; set; }
        public string Batch { get; set; }
        public string SupervisorID { get; set; }
        public string CoSupervisorID { get; set; }
        public string companyID { get; set; }
        public string CordinatorID { get; set; }

    }
}
