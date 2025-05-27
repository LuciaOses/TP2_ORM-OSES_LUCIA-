
namespace Application.Response
{
    public class ApprovalStep
    {
        public int? StepOrder { get; set; }
        public required string? ApproverUser { get; set; }
        public required string ApproverRole { get; set; }
        public required string Status { get; set; }
        public DateTime? DecisionDate { get; set; }
        public string? Observations { get; set; }
    }
}
