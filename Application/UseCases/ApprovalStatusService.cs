using Application.Interfaces.IApprovalStatus;
using Domain.Entities;


namespace Application.UseCases
{
    public class ApprovalStatusService : IApprovalStatusService
    {
        private readonly IApprovalStatusQuery _query;

        public ApprovalStatusService(IApprovalStatusQuery query)
        {
            _query = query;
        }

        public async Task<List<ApprovalStatus>> GetAllAsync()
        {
            return await _query.GetAllStatuses();
        }
    }
}
