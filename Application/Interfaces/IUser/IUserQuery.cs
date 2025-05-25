using Domain.Entities;

namespace Application.Interfaces.IUser
{
    public interface IUserQuery
    {
        Task<List<User>> GetAllUsers();
    }
}
