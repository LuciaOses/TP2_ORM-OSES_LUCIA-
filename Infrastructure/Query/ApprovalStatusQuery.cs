using Application.Interfaces.IApprovalStatus;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Query
{
    public class ApprovalStatusQuery(AprobacionDbContext context) : IApprovalStatusQuery
    {
        private readonly AprobacionDbContext _context = context;

        public async Task<List<ApprovalStatus>> GetAllStatuses()
        {
            return await _context.ApprovalStatuses.ToListAsync();
        }
    }
}
