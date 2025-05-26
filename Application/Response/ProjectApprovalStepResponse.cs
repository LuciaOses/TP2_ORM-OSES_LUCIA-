using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response
{
    public class ProjectApprovalStepResponse
    {
        public int? StepOrder { get; set; }
        public string? ApproverUser { get; set; }
        public string ApproverRole { get; set; }
        public string Status { get; set; }
        public DateTime? DecisionDate { get; set; }
        public string? Observations { get; set; }
    }
}
