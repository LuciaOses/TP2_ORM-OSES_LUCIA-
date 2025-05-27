
namespace Application.Request
{
    public class DecisionStep
    {
        public int Id { get; set; } 
        public int User { get; set; } 
        public int Status { get; set; } 
        public string Observation { get; set; } = string.Empty; 
    }
}
