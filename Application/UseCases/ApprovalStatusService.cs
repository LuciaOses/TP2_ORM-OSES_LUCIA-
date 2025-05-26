using Application.Interfaces.IApprovalStatus;
using Domain.Entities;


namespace Application.UseCases
{
    public class ApprovalStatusService(IApprovalStatusQuery query) : IApprovalStatusService
    {
        private readonly IApprovalStatusQuery _query = query;

        public async Task<List<ApprovalStatus>> GetAllAsync()
        {
            return await _query.GetAllStatuses();
        }
    }
}
