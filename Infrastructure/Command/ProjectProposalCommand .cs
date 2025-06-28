using Application.Interfaces.IProjectProporsal;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Query
{
    public class ProjectProposalCommand : IProjectProposalCommand
    {
        private readonly AprobacionDbContext _context;

        public ProjectProposalCommand(AprobacionDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByTitle(string title)
        {
            var normalizedTitle = title.Trim().ToLower();
            return await _context.ProjectProposals.AnyAsync(p => p.Title.ToLower() == normalizedTitle);
        }

        public async Task AddAsync(ProjectProposal proposal)
        {
            _context.ProjectProposals.Add(proposal);
            await _context.SaveChangesAsync();
        }

        public IQueryable<ProjectProposal> Query()
        {
            return _context.ProjectProposals
                .Include(p => p.AreaNavigation)
                .Include(p => p.TypeNavigation)
                .Include(p => p.StatusNavigation)
                .Include(p => p.CreateByNavigation)
                .Include(p => p.ApprovalSteps);
        }
        public async Task<ProjectProposal?> GetByIdWithStepsAsync(Guid id)
        {
            return await _context.ProjectProposals
                .Include(p => p.AreaNavigation)
                .Include(p => p.TypeNavigation)
                .Include(p => p.StatusNavigation)
                .Include(p => p.CreateByNavigation)
                    .ThenInclude(u => u.ApproverRole)
                .Include(p => p.ApprovalSteps)
                    .ThenInclude(s => s.ApproverUser)
                        .ThenInclude(u => u.ApproverRole)
                .Include(p => p.ApprovalSteps)
                    .ThenInclude(s => s.ApproverRole)
                .Include(p => p.ApprovalSteps)
                    .ThenInclude(s => s.StatusNavigation)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task UpdateAsync(ProjectProposal proposal)
        {
            _context.ProjectProposals.Update(proposal);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
