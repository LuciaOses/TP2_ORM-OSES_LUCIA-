using Domain.Entities;


namespace Application.Interfaces.IUser
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
    }
}
