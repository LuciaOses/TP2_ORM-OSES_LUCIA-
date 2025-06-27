using Domain.Entities;
using Application.Response;

namespace Application.Mappers
{
    public static class ProjectTypeMapper
    {
        public static GenericResponse? ToDto(ProjectType? type)
        {
            if (type == null)
                return null;

            return new GenericResponse
            {
                Id = type.Id,
                Name = type.Name
            };
        }

        public static IEnumerable<GenericResponse?> ToDtoList(IEnumerable<ProjectType>? types)
        {
            if (types == null)
                return Enumerable.Empty<GenericResponse?>();

            return types.Select(ToDto);
        }
    }
}
