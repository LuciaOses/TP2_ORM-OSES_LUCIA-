using Application.Response;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Application.Mappers
{
    public static class AreaMapper
    {
        public static GenericResponse ToDto(Area area) => new GenericResponse
        {
            Id = area.Id,
            Name = area.Name
        };

        public static IEnumerable<GenericResponse> ToDtoList(IEnumerable<Area> areas) =>
            areas.Select(ToDto);
    }
}
