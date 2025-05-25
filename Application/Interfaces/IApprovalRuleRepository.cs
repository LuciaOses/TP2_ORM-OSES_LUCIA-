using Domain.Entities;

namespace Application.Interfaces;

public interface IApprovalRuleRepository
{
    Task<List<ApprovalRule>> GetRulesByAmountAsync(decimal amount);
}
