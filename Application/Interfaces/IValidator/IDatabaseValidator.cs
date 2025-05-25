using Domain.Entities;

namespace Application.Interfaces.IValidator
{
    public interface IDatabaseValidator
    {
        Task<User> GetUserById(int userId);
        Task ValidateUserExistsAsync(int userId);
        Task ValidateAreaExistsAsync(int areaId);
    }
}
