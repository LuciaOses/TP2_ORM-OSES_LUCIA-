using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Request
{
    public class ProjectFilterRequest
    {
        public string? Title { get; set; }
        public int? Status { get; set; }
        public int? Applicant { get; set; }
        public int? ApprovalUser { get; set; }
    }
}
