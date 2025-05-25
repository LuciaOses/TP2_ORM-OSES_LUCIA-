namespace Application.Interfaces
{
    public interface IUseCaseValidator
    {
        void ValidateUserPermissions(int userRoleId, int requiredRoleId);
    }
}