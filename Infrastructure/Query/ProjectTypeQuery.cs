using Application.Interfaces.IProjectType;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Query
{
    public class ProjectTypeQuery(AprobacionDbContext context) : IProjectTypeQuery
    {
        private readonly AprobacionDbContext _context = context;

        public async Task<List<ProjectType>> GetAllProjectTypes()
        {
            return await _context.ProjectTypes.ToListAsync();
        }
    }
}
