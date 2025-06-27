
namespace Application.Response
{
    public class ProjectProposalResponseDetail
    {
        public Guid Id { get; set; }
        public string? Title { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public double Amount { get; set; }
        public int Duration { get; set; }
        public required UserResponse? User { get; set; }
        public required GenericResponse? Area { get; set; }
        public required GenericResponse? Status { get; set; }
        public required GenericResponse? Type { get; set; }
        public List<ApprovalStepResponse> Steps { get; set; } = new();
        public static ProjectProposalResponseDetail Conflict => new()
        {
            Id = Guid.Empty,
            Title = null,
            Description = null,
            Amount = 0,
            Duration = 0,
            User = null,
            Area = null,
            Status = null,
            Type = null,
            Steps = []
        };
    }
}
