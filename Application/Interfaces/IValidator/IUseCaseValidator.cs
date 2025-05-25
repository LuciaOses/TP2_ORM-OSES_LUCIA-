namespace Application.Interfaces.IValidator
{
    public interface IUseCaseValidator
    {
        void ValidateUserPermissions(int userRoleId, int requiredRoleId);
    }
}