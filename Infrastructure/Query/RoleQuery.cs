using Application.Interfaces.IRole;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Query
{
    public class RoleQuery : IRoleQuery
    {
        private readonly AprobacionDbContext _context;

        public RoleQuery(AprobacionDbContext context)
        {
            _context = context;
        }

        public async Task<List<ApproverRole>> GetAllRoles()
        {
            return await _context.ApproverRoles.ToListAsync();
        }
    }
}
