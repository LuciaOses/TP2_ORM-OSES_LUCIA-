using Application.Request;
using Application.Response;

namespace Application.Interfaces.IProjectProporsal
{
    public interface IProjectProposalService
    {
        Task<Project> CreateProjectProposal(string title, string? description, int areaId, int typeId, decimal amount, int duration, int userId);
        Task<bool> ExistingProject(string title);
        Task<IEnumerable<Project>> SearchProjects(ProjectFilterRequest filters);
        Task<Project> TakeDecision(Guid projectId, DecisionStep request);
    }
}
