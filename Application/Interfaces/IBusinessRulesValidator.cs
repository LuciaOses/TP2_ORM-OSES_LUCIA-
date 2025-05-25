using Domain.Entities;

namespace Application.Interfaces
{
    public interface IBusinessRulesValidator
    {
        void ValidateProposalStatus(ProjectProposal proposal);
        void ValidateNoRejectedSteps(IEnumerable<ProjectApprovalStep> steps);
    }
}