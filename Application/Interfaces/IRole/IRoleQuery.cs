using Domain.Entities;

namespace Application.Interfaces.IRole
{
    public interface IRoleQuery
    {
        Task<List<ApproverRole>> GetAllRoles();
    }
}
