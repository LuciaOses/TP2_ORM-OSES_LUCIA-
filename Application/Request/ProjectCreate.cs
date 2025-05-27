

namespace Application.Request
{
    public class ProjectCreate
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Area { get; set; }
        public int Type { get; set; }
        public decimal Amount { get; set; }
        public int Duration { get; set; }
        public int User { get; set; }
    }
}
