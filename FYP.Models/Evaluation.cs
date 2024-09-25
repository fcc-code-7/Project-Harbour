using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Entities
{
    public class Evaluation : BaseEntity
    {
        public int Marks { get; set; }
        public string? PBatch { get; set; }

        public DateTime LastDate { get; set; } = DateTime.Now;

        public string? EvaluationName { get; set; }

    }
}
