using Application.Exceptions;
using Application.Interfaces.IValidator;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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
                throw new ExceptionBadRequest("El usuario no existe.");
        }

        public async Task ValidateAreaExistsAsync(int areaId)
        {
            var exists = await _dbContext.Areas.AnyAsync(a => a.Id == areaId);
            if (!exists)
                throw new ExceptionBadRequest("El área no existe.");
        }

        public async Task<bool> ProjectTypeExists(int typeId)
        {
            return await _dbContext.ProjectTypes.AnyAsync(t => t.Id == typeId);
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId)
                ?? throw new ExceptionBadRequest("El usuario no fue encontrado.");
        }

        public async Task<Area?> GetAreaByIdAsync(int areaId)
        {
            return await _dbContext.Areas.FindAsync(areaId);
        }

        public async Task<ProjectType?> GetProjectTypeByIdAsync(int typeId)
        {
            return await _dbContext.ProjectTypes.FindAsync(typeId);
        }
    }
}

