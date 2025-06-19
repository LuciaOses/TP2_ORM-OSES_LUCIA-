using Domain.Entities;

namespace Application.Interfaces;

public interface IApprovalRuleQuery
{
    Task<List<ApprovalRule>> GetRulesByAmountAsync(decimal amount);
}
