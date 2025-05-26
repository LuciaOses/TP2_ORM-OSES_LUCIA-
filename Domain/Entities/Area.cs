

namespace Domain.Entities
{
    public class Area
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<ProjectProposal> ProjectProposals { get; set; } = [];
        public virtual ICollection<ApprovalRule> ApprovalRules { get; set; } = [];
    }
}
