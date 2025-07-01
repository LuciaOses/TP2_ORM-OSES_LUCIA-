using Application.Response;
using Domain.Entities;


namespace Application.Mappers
{
    public static class UserMapper
    {
        public static UserResponse ToDto(User? user)
        {
            return user == null
                ? new UserResponse
                {
                    Id = 0,
                    Name = string.Empty,
                    Email = string.Empty,
                    Role = new GenericResponse { Id = 0, Name = string.Empty }
                }
                : new UserResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = RoleMapper.ToDto(user.ApproverRole)
                };
        }
        public static List<UserResponse?> ToDtoList(List<User> users)
        {
            return users.Select(ToDto).ToList();
        }
    }
}