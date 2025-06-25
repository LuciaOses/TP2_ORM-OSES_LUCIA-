using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Interfaces.IApprovalStatus
{
    public interface IApprovalStatusQuery
    {
        Task<List<ApprovalStatus>> GetAllStatuses();
    }
}
