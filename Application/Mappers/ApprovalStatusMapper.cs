using Domain.Entities;
using Application.Response;

namespace Application.Mappers
{
    public static class ApprovalStatusMapper
    {
        public static GenericResponse ToDto(ApprovalStatus status) => new GenericResponse
        {
            Id = status.Id,
            Name = status.Name
        };

        public static IEnumerable<GenericResponse> ToDtoList(IEnumerable<ApprovalStatus> statuses) =>
            statuses.Select(ToDto);
    }
}

