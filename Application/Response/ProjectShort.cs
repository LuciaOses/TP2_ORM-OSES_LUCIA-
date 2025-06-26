
namespace Application.Response
{
    public class ProjectShort
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Area { get; set; }
        public required string Type { get; set; }
        public decimal Amount { get; set; }
        public int Duration { get; set; }
        public required string Status { get; set; }
        
    }
}
