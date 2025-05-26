

namespace Domain.Entities
{
    public class ApprovalStatus
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<ProjectProposal> ProjectProposals { get; set; } = [];
        public virtual ICollection<ProjectApprovalStep> ProjectApprovalSteps { get; set; } = [];
    }
}
