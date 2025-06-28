using Domain.Entities;

namespace Application.Interfaces.IProjectProporsal
{
    public interface IProjectProposalCommand
    {
        Task<bool> ExistsByTitle(string title);
        Task<bool> ExistsByTitleExceptIdAsync(string title, Guid excludeId);
        Task AddAsync(ProjectProposal proposal);
        IQueryable<ProjectProposal> Query();
        Task<ProjectProposal?> GetByIdWithStepsAsync(Guid id);
        Task UpdateAsync(ProjectProposal proposal);
        Task SaveChangesAsync();
    }
}
