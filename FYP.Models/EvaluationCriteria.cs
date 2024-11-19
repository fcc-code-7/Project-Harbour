using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Entities
{
    public class EvaluationCriteria : BaseEntity
    {
        public string? GId { get; set; }
        public string? Batch { get; set; }
        public string? EvalName { get; set; }
        public string? Q1Marks { get; set; }

        public string? Q2Marks { get; set; }

        public string? Q3Marks { get; set; }

        public string? Q4Marks { get; set; }

        public string? Q5Marks { get; set; }
        public string? Q6Marks { get; set; }
        public string? Q7Marks { get; set; }
        public string? Q8Marks { get; set; }
        public string? Remarks { get; set; }
        public string? CommiteeID { get; set; }
        public decimal TotalMarks { get; set; }
        public DateTime? SubmissionTime { get; set; }
        public DateTime? SubmissionDate { get; set; }


    }
}
