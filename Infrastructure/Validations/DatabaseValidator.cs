using Application.Interfaces.IValidator;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infraestructura.Validations
{
    public class DatabaseValidator : IDatabaseValidator
    {
        private readonly AprobacionDbContext _dbContext;

        public DatabaseValidator(AprobacionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ValidateUserExistsAsync(int userId)
        {
            var exists = await _dbContext.Users.AnyAsync(u => u.Id == userId);
            if (!exists)
                throw new ValidationException("El usuario no existe.");
        }

        public async Task ValidateAreaExistsAsync(int areaId)
        {
            var exists = await _dbContext.Areas.AnyAsync(a => a.Id == areaId);
            if (!exists)
                throw new ValidationException("El área no existe.");
        }

        public async Task<bool> ProjectTypeExists(int typeId)
        {
            return await _dbContext.ProjectTypes.AnyAsync(t => t.Id == typeId);
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId)
                ?? throw new ValidationException("El usuario no fue encontrado.");
        }
    }
}

