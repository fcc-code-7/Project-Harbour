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
        public string? Q1 { get; set; }

        public string? Q2 { get; set; }

        public string? Q3 { get; set; }

        public string? Q4 { get; set; }

        public string? Q5 { get; set; }
        public string? Q6 { get; set; }
        public string? Q7 { get; set; }
        public string? Q8 { get; set; }
        public int? StudentsProposalMarks { get; set; }
        public int? Student1MidMarks { get; set; }
        public int? Student2MidMarks { get; set; }
        public int? Student3MidMarks { get; set; }
        public int? Student1FinalMarks { get; set; }
        public int? Student2FinalMarks { get; set; }
        public int? Student3FinalMarks { get; set; }
        public int? CordinatorMarks { get; set; }
        public int? SupervisorMarks { get; set; }
        public string? Remarks { get; set; }
        public string? CommiteeID { get; set; }
        public int TotalMarks { get; set; }
        public DateTime? SubmissionTime { get; set; }
        public DateTime? SubmissionDate { get; set; }


    }
}
