using Application.Request;
using Application.Response;
using Domain.Entities;

namespace Application.Interfaces.IProjectProporsal
{
    public interface IProjectProposalService
    {
        Task<ProjectProposal> CreateProjectProposal(string title, string description, int areaId, int typeId, decimal amount, int duration, int userId);
        Task<bool> ExistingProject(string title);
        Task<IEnumerable<ProjectShort>> SearchProjects(ProjectFilterRequest filters);
        Task<ProjectShort> TakeDecision(Guid projectId, DecisionStep request);
    }
}
