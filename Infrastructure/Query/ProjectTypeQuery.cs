using Application.Interfaces.IProjectType;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Query
{
    public class ProjectTypeQuery : IProjectTypeQuery
    {
        private readonly AprobacionDbContext _context;

        public ProjectTypeQuery(AprobacionDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProjectType>> GetAllProjectTypes()
        {
            return await _context.ProjectTypes.ToListAsync();
        }
    }
}
