
namespace Application.Response
{
    public class ApprovalStep
    {
        public long Id { get; set; }
        public int StepOrder { get; set; }
        public DateTime? DecisionDate { get; set; }
        public required string Observations { get; set; }

        public required Users ApproverUser { get; set; }
        public required GenericResponse ApproverRole { get; set; }
        public required GenericResponse Status { get; set; }
    }
    
}
