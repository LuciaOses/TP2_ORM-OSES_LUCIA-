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
            return await _context.ProjectProposals.AnyAsync(p => p.Title == title);
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
                .Include(p => p.ApprovalSteps)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateAsync(ProjectProposal proposal)
        {
            _context.ProjectProposals.Update(proposal);

        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<ProjectProposal?> GetProjectWithStepsByIdAsync(Guid id)
        {
            return await _context.ProjectProposals
                .Include(p => p.AreaNavigation)
                .Include(p => p.TypeNavigation)
                .Include(p => p.StatusNavigation)
                .Include(p => p.CreateByNavigation)
                .Include(p => p.ApprovalSteps)
                    .ThenInclude(s => s.ApproverUser)
                .Include(p => p.ApprovalSteps)
                    .ThenInclude(s => s.ApproverRole)
                .Include(p => p.ApprovalSteps)
                    .ThenInclude(s => s.StatusNavigation)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
