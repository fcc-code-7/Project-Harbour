using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Entities
{
    public class Project : BaseEntity
    {
        public string? code { get; set; }
        public string? Title { get; set; }
        public string? ProjectCategory { get; set; }
        public string? projectGroup { get; set; }
        public string? Others { get; set; }
        public string? groupId { get; set; }
        public string? Specialization { get; set; }
        public string? Tools { get; set; }
        public string? companyID { get; set; }
        public string? batch { get; set; }
        public string? Summary { get; set; }
        public string? objectives { get; set; }
        public string? ExpectedResults { get; set; }
        public string? commiteeId { get; set; }
        public string? changeTitleFormStatus { get; set; }
        public string? SupervsiorApproved { get; set; }
        public string? Status { get; set; }
        public string? PropPPT { get; set; }
        public string? PropReport { get; set; }
        public string? MidPPT { get; set; }
        public string? MidReport { get; set; }
        public string? FinalDocs { get; set; }
        public int TotalMarks { get; set; }

    }

}
