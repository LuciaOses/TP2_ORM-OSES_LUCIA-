using Domain.Entities;

namespace Application.Interfaces
{
    public interface IDatabaseValidator
    {
        Task<User> GetUserById(int userId);
        Task ValidateUserExistsAsync(int userId);
        Task ValidateAreaExistsAsync(int areaId);
    }
}
