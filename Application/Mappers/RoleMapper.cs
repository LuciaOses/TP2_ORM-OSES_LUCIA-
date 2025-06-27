using Domain.Entities;
using Application.Response;

namespace Application.Mappers
{
    public static class RoleMapper
    {
        public static GenericResponse ToDto(ApproverRole? role)
        {
            return role == null
                ? new GenericResponse { Id = 0, Name = string.Empty }
                : new GenericResponse { Id = role.Id, Name = role.Name };
        }

        public static IEnumerable<GenericResponse?> ToDtoList(IEnumerable<ApproverRole> roles)
        {
            if (roles == null)
                return Enumerable.Empty<GenericResponse>();

            return roles.Select(ToDto);
        }
    }
}
