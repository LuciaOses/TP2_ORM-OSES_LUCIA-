

namespace Domain.Entities
{
    public class ProjectProposal
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public decimal EstimatedAmount { get; set; }
        public int EstimatedDuration { get; set; }
        public DateTime CreateAt { get; set; }

        public int Area { get; set; }
        public virtual Area? AreaNavigation { get; set; }

        public int Type { get; set; }
        public virtual ProjectType? TypeNavigation { get; set; }

        public int Status { get; set; }
        public virtual ApprovalStatus? StatusNavigation { get; set; }

        public int CreateBy { get; set; }
        public virtual User? CreateByNavigation { get; set; }

        public virtual ICollection<ProjectApprovalStep> ApprovalSteps { get; set; } = [];
    }
}
