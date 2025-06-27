using Application.Response;
using Domain.Entities;


namespace Application.Mappers
{
    public static class UserMapper
    {
        public static Users? ToDto(User user)
        {
            return user == null
                ? null
                : new Users
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = RoleMapper.ToDto(user.ApproverRole),
                };
        }
        public static List<Users?> ToDtoList(List<User> users)
        {
            return users.Select(ToDto).ToList();
        }
    }
}

