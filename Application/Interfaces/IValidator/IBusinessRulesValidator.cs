using Domain.Entities;

namespace Application.Interfaces.IValidator
{
    public interface IBusinessRulesValidator
    {
        void ValidateProposalStatus(ProjectProposal proposal);
        void ValidateNoRejectedSteps(IEnumerable<ProjectApprovalStep> steps);
    }
}