using Application.Interfaces.IValidator;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Validations
{
    public class DatabaseValidator(AprobacionDbContext dbContext) : IDatabaseValidator
    {
        private readonly AprobacionDbContext _dbContext = dbContext;

        public async Task ValidateUserExistsAsync(int userId)
        {
            var userExists = await _dbContext.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
                throw new Exception("El usuario no existe.");
        }

        public async Task ValidateAreaExistsAsync(int areaId)
        {
            var areaExists = await _dbContext.Areas.AnyAsync(a => a.Id == areaId);
            if (!areaExists)
                throw new Exception("El área no existe.");
        }

        public Task<User> GetUserById(int userId)
        {
            throw new NotImplementedException();
        }
    }
}

