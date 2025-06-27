using Domain.Entities;

namespace Application.Interfaces.IValidator
{
    public interface IDatabaseValidator
    {
        Task ValidateUserExistsAsync(int userId);
        Task ValidateAreaExistsAsync(int areaId);
        Task<User> GetUserById(int userId);
        Task<bool> ProjectTypeExists(int typeId);

        Task<Area?> GetAreaByIdAsync(int areaId);
        Task<ProjectType?> GetProjectTypeByIdAsync(int typeId);

    }
}
