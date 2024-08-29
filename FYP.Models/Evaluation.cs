using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Entities
{
    public class Evaluation
    {
        public string PDefId { get; set; }
        public int Marks { get; set; }
        public string Remarks { get; set; }


        public string CommiteeID { get; set; }

        public string EvaluationID { get; set; }
    }
}
