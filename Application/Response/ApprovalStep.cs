
namespace Application.Response
{
    public class ApprovalStep
    {
        public int? StepOrder { get; set; }
        public string? ApproverUser { get; set; }
        public string ApproverRole { get; set; }
        public string Status { get; set; }
        public DateTime? DecisionDate { get; set; }
        public string? Observations { get; set; }
    }
}
