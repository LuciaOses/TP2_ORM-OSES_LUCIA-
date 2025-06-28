using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Command;

public class ApprovalRuleQuery : IApprovalRuleQuery
{
    private readonly AprobacionDbContext _context;

    public ApprovalRuleQuery(AprobacionDbContext context)
    {
        _context = context;
    }

    public async Task<List<ApprovalRule>> GetRulesByAmountAsync(decimal amount)
    {
        return await _context.ApprovalRules
            .Where(r =>
                r.MinAmount <= amount &&
                (r.MaxAmount == 0 || amount <= r.MaxAmount))
            .ToListAsync();
    }
}