using Application.Interfaces.IUser;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Command
{
    public class UserCommand : IUserCommand
    {
        private readonly AprobacionDbContext _context;

        public UserCommand(AprobacionDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync() =>
            await _context.Users.ToListAsync();

        public async Task<User?> GetByIdAsync(int id) =>
            await _context.Users.FindAsync(id);

        public async Task AddAsync(User user) =>
            await _context.Users.AddAsync(user);

        public async Task UpdateAsync(User user) =>
            _context.Users.Update(user);

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
                _context.Users.Remove(user);
        }
        public async Task<List<User>> GetByRoleAsync(int roleId) =>
            await _context.Users.Where(u => u.Role == roleId).ToListAsync();
    }
}
