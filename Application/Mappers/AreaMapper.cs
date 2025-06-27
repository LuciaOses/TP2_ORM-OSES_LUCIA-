using Application.Response;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Application.Mappers
{
    public static class AreaMapper
    {
        public static GenericResponse? ToDto(Area? area)
        {
            if (area == null)
                return null;

            return new GenericResponse
            {
                Id = area.Id,
                Name = area.Name
            };
        }

        public static IEnumerable<GenericResponse?> ToDtoList(IEnumerable<Area>? areas)
        {
            if (areas == null)
                return Enumerable.Empty<GenericResponse?>();

            return areas.Select(ToDto);
        }
    }
}