using Domain.Entities;
using Application.Response;

namespace Application.Mappers
{
    public static class ApprovalStatusMapper
    {
        // Mapea un solo ApprovalStatus a GenericResponse, manejando nulos
        public static GenericResponse? ToDto(ApprovalStatus? status)
        {
            if (status == null)
                return null;

            return new GenericResponse
            {
                Id = status.Id,
                Name = status.Name
            };
        }

        // Mapea una lista de ApprovalStatus a una lista de GenericResponse, manejando nulos
        public static IEnumerable<GenericResponse?> ToDtoList(IEnumerable<ApprovalStatus>? statuses)
        {
            if (statuses == null)
                return Enumerable.Empty<GenericResponse?>();

            return statuses.Select(ToDto);
        }
    }
}


