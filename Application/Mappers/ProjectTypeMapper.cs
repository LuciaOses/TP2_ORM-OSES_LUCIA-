using Domain.Entities;
using Application.Response;
using System.Collections.Generic;
using System.Linq;

namespace Application.Mappers
{
    public static class ProjectTypeMapper
    {
        public static GenericResponse ToDto(ProjectType? type)
        {
            return type == null
                ? new GenericResponse { Id = 0, Name = string.Empty }
                : new GenericResponse { Id = type.Id, Name = type.Name };
        }

        public static IEnumerable<GenericResponse> ToDtoList(IEnumerable<ProjectType>? types)
        {
            if (types == null)
                return Enumerable.Empty<GenericResponse>();

            return types.Select(ToDto);
        }
    }
}
