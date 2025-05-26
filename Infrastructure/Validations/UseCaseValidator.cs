using Application.Interfaces.IValidator;

namespace Infrastructure.Validations
{
    public class UseCaseValidator : IUseCaseValidator
    {
        public void ValidateUserPermissions(int userRoleId, int requiredRoleId)
        {
            if (userRoleId != requiredRoleId)
            {
                throw new UnauthorizedAccessException("No tiene permiso para realizar esta acción.");
            }
        }
    }
}
