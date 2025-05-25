using Domain.Entities;

namespace Application.Interfaces;

public interface IProjectApprovalStepRepository
{
    Task AddAsync(ProjectApprovalStep step);
    Task<ProjectApprovalStep?> GetByIdAsync(int id);
    Task UpdateAsync(ProjectApprovalStep step);
    Task SaveChangesAsync();
}
