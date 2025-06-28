using Application.Request;
using Application.Response;
using Domain.Entities;

namespace Application.Interfaces.IProjectProporsal
{
    public interface IProjectProposalService
    {
        Task<ProjectProposalResponseDetail> CreateProjectProposal(ProjectCreate request);
        Task<bool> ExistsByTitle(string title);
        Task<IEnumerable<ProjectShort>> SearchProjects(ProjectFilterRequest filters);
        Task<ProjectProposalResponseDetail> TakeDecision(Guid projectId, DecisionStep request);
    }
}
