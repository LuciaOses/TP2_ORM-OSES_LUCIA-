using Domain.Entities;
using Application.Response;

namespace Application.Mappers
{
    public static class RoleMapper
    {
        public static GenericResponse ToDto(ApproverRole role) => new GenericResponse
        {
            Id = role.Id,
            Name = role.Name
        };

        public static IEnumerable<GenericResponse> ToDtoList(IEnumerable<ApproverRole> roles) =>
            roles.Select(ToDto);
    }
}
