using Application.Response;
using Domain.Entities;


namespace Application.Mappers
{
    public static class UserMapper
    {
        public static UsersResponse ToDto(User user) => new UsersResponse
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

        public static IEnumerable<UsersResponse> ToDtoList(IEnumerable<User> users) =>
            users.Select(ToDto);
    }

}
