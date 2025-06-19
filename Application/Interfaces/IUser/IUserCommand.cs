using Domain.Entities;

namespace Application.Interfaces.IUser
{
    public interface IUserCommand
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task<List<User>> GetByRoleAsync(int roleId);
    }
}
