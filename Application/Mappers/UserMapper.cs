using Application.Response;
using Domain.Entities;


namespace Application.Mappers
{
    public static class UserMapper
    {
        public static Users ToDto(User user) => new Users
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,

            Role = new GenericResponse
            {
                Id = user.ApproverRole.Id,
                Name = user.ApproverRole.Name
            }
        };

        public static IEnumerable<Users> ToDtoList(IEnumerable<User> users) =>
            users.Select(ToDto);
    }

}
