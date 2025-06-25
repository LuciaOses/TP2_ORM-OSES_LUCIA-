using Domain.Entities;
using Application.Response;

namespace Application.Mappers
{
    public static class ProjectTypeMapper
    {
        public static GenericResponse ToDto(ProjectType type) => new GenericResponse
        {
            Id = type.Id,
            Name = type.Name
        };

        public static IEnumerable<GenericResponse> ToDtoList(IEnumerable<ProjectType> types) =>
            types.Select(ToDto);
    }
}

