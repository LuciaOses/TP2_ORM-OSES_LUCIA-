using Domain.Entities;
using Application.Response;

namespace Application.Mappers
{
    public static class ApprovalStatusMapper
    {

        public static GenericResponse ToDto(ApprovalStatus? status)
        {
            return status == null
                ? new GenericResponse { Id = 0, Name = string.Empty }
                : new GenericResponse { Id = status.Id, Name = status.Name };
        }
        public static IEnumerable<GenericResponse?> ToDtoList(IEnumerable<ApprovalStatus>? statuses)
        {
            if (statuses == null)
                return Enumerable.Empty<GenericResponse?>();

            return statuses.Select(ToDto);
        }
    }
}