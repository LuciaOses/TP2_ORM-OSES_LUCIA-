using Domain.Entities;

namespace Application.Interfaces.IRole
{
    public interface IRoleService
    {
        Task<List<ApproverRole>> GetAllAsync();
    }

}
