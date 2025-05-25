using Domain.Entities;

namespace Application.Interfaces.IApprovalStatus
{
    public interface IApprovalStatusQuery
    {
        Task<List<ApprovalStatus>> GetAllStatuses();
    }
}
