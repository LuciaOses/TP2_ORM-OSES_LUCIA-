using Application.Interfaces.IRole;
using Domain.Entities;

namespace Application.UseCases
{
    public class RoleService : IRoleService
    {
        private readonly IRoleQuery _query;

        public RoleService(IRoleQuery query)
        {
            _query = query;
        }

        public async Task<List<ApproverRole>> GetAllAsync()
        {
            return await _query.GetAllRoles();
        }
    }
}
