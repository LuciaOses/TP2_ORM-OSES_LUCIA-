namespace Domain.Entities
{
    public class ProjectApprovalStep
    {
        public long Id { get; set; }
        public int StepOrder { get; set; }
        public DateTime? DecisionDate { get; set; }
        public string? Observations { get; set; }

        public Guid ProjectProposalId { get; set; }
        public virtual ProjectProposal? ProjectProposal { get; set; }

        public int? ApproverUserId { get; set; }
        public virtual User? ApproverUser { get; set; }

        public int ApproverRoleId { get; set; }
        public virtual ApproverRole? ApproverRole { get; set; }

        public int Status { get; set; }
        public virtual ApprovalStatus? StatusNavigation { get; set; }
    }
}
