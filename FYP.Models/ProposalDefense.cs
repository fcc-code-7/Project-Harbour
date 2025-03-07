﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Entities
{
    public class ProposalDefense : BaseEntity
    {
        public string? PropID { get; set; }
        public string? Q1 { get; set; }
        public string? Q2 { get; set; }
        public string? Q3 { get; set; }
        public string? Q4 { get; set; }
        public string? Q5 { get; set; }
        public string? Q6 { get; set; }
        public string? Q7 { get; set; }
        public string? Q8 { get; set; }
        public string? Q9 { get; set; }
        public string? Q10 { get; set; }
        public bool Status { get; set; }
        public string? Remarks { get; set; }

        public int Marks { get; set; }

        public string? CommiteeID { get; set; }

    }
}
