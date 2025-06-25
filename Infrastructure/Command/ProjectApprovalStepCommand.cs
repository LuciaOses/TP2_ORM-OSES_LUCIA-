using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Command
{
    public class ProjectApprovalStepCommand : IProjectApprovalStepCommand
    {
        private readonly AprobacionDbContext _context;

        public ProjectApprovalStepCommand(AprobacionDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ProjectApprovalStep step)
        {
            await _context.ProjectApprovalSteps.AddAsync(step);
        }

        public async Task<ProjectApprovalStep?> GetByIdAsync(int id)
        {
            return await _context.ProjectApprovalSteps.FindAsync(id);
        }

        public async Task UpdateAsync(ProjectApprovalStep step)
        {
            _context.ProjectApprovalSteps.Update(step);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

