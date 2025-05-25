using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProjectProposalRepository
    {
        Task<List<ProjectProposal>> GetAllAsync();
        Task<ProjectProposal?> GetByIdAsync(Guid id);
        Task<ProjectProposal?> GetByIdWithEstadoAsync(Guid id);
        Task AddAsync(ProjectProposal proposal);
        Task UpdateAsync(ProjectProposal proposal);
    }
}

