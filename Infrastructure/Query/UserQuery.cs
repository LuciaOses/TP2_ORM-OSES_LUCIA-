using Application.Interfaces.IUser;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Query
{
    public class UserQuery : IUserQuery
    {
        private readonly AprobacionDbContext _context;

        public UserQuery(AprobacionDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
