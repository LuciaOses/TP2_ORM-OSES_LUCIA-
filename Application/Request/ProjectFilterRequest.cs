

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
