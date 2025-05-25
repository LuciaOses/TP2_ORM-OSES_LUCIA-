namespace Application.Interfaces
{
    public interface ICurrentUser
    {
        int GetCurrentRoleId();
        int? GetCurrentUserId();
    }
}
