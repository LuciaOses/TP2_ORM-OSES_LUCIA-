using Domain.Entities;

namespace Application.Interfaces.IApprovalStatus
{
    public interface IApprovalStatusService
    {
        Task<List<ApprovalStatus>> GetAllAsync();
    }
}
