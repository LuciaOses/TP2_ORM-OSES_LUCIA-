
namespace Application.Response
{
    public class ProjectDetailResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal EstimatedAmount { get; set; }
        public int EstimatedDuration { get; set; }
        public string Area { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateAt { get; set; }

        public List<ApprovalStep> ApprovalSteps { get; set; }

        public static readonly ProjectDetailResponse Conflict = new ProjectDetailResponse { Status = "-1" };
    }
}
