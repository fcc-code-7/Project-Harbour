using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Entities
{
    public class EvaluationCriteria : BaseEntity
    {
        public string? PId { get; set; }
        public string? EvalName { get; set; }
        public string? Q1 { get; set; }
        public string? Q1Desc { get; set; }
        public string? Q1Marks { get; set; }
        public string? Q2 { get; set; }
        public string? Q2Desc { get; set; }

        public string? Q2Marks { get; set; }
        public string? Q3 { get; set; }
        public string? Q3Desc { get; set; }

        public string? Q3Marks { get; set; }
        public string Q4 { get; set; }
        public string? Q4Desc { get; set; }

        public string? Q4Marks { get; set; }
        public string? Q5 { get; set; }
        public string? Q5Desc { get; set; }

        public string? Q5Marks { get; set; }
        public string? Remarks { get; set; }
        public string? CommiteeID { get; set; }

        public int TotalMarks { get; set; }


    }
}
