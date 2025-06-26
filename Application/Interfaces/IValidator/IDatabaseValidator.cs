using Domain.Entities;

namespace Application.Interfaces.IValidator
{
    public interface IDatabaseValidator
    {
        Task ValidateUserExistsAsync(int userId);
        Task ValidateAreaExistsAsync(int areaId);
        Task<User> GetUserById(int userId);
        Task<bool> ProjectTypeExists(int typeId);
        Task<bool> AreaExistsAsync(int areaId);
        Task<bool> UserExistsAsync(int userId);

    }
}
