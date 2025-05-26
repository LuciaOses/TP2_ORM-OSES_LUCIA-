using Application.Request;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IProjectProporsal
{
    public interface IProjectProposalService
    {
        Task<ProjectProposalResponseDetail> CreateProjectProposal(string title, string? description, int areaId, int typeId, decimal amount, int duration, int userId);
        Task<bool> ExistingProject(string title);
        Task<IEnumerable<ProjectProposalResponseDetail>> SearchProjects(ProjectFilterRequest filters);
        Task<ProjectProposalResponseDetail> TakeDecision(Guid projectId, DecisionStepRequest request);
    }
}
