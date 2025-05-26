using Domain.Entities;

namespace Application.Interfaces.IValidator
{
    public interface IDatabaseValidator
    {
        Task ValidateUserExistsAsync(int userId);
        Task ValidateAreaExistsAsync(int areaId);
        Task<bool> ProjectTypeExists(int typeId);
        Task<User> GetUserById(int userId);
    }
}
