using Application.Interfaces.IApprovalStatus;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Command
{
    public class ApprovalStatusQuery : IApprovalStatusQuery
    {
        private readonly AprobacionDbContext _context;

        public ApprovalStatusQuery(AprobacionDbContext context)
        {
            _context = context;
        }

        public async Task<List<ApprovalStatus>> GetAllStatuses()
        {
            return await _context.ApprovalStatuses.ToListAsync();
        }
    }
}