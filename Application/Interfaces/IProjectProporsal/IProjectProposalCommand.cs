using Domain.Entities;

namespace Application.Interfaces.IProjectProporsal
{
    public interface IProjectProposalCommand
    {
        Task<bool> ExistsByTitle(string title);
        Task AddAsync(ProjectProposal proposal);
        IQueryable<ProjectProposal> Query();
        Task<ProjectProposal?> GetByIdWithStepsAsync(Guid id);
        Task UpdateAsync(ProjectProposal proposal);
        Task SaveChangesAsync();
    }
}
