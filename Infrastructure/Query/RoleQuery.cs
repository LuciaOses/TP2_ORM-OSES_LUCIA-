using Application.Interfaces.IRole;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Query
{
    public class RoleQuery(AprobacionDbContext context) : IRoleQuery
    {
        private readonly AprobacionDbContext _context = context;

        public async Task<List<ApproverRole>> GetAllRoles()
        {
            return await _context.ApproverRoles.ToListAsync();
        }
    }
}
