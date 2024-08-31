using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Entities
{
    public class Evaluation : BaseEntity
    {
        public string? PId { get; set; }
        public int Marks { get; set; }

        public DateTime LastDate { get; set; }

        public string? EvaluationName { get; set; }

    }
}
