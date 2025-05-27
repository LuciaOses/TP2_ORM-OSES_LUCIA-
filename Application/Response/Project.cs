
namespace Application.Response
{
    public class Project
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Area { get; set; }
        public required string Type { get; set; }
        public decimal EstimatedAmount { get; set; }
        public int EstimatedDuration { get; set; }
        public required string Status { get; set; }
        public required DateTime CreateDate { get; set; }
        public required string CreatedBy { get; set; }
    }
}
