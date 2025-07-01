using Application.Interfaces.IUser;
using Domain.Entities;


namespace Application.UseCases
{
    public class UserService(IUserQuery query) : IUserService
    {
        private readonly IUserQuery _query = query;

        public async Task<List<User>> GetAllAsync()
        {
            return await _query.GetAllUsers();
        }
    }
}