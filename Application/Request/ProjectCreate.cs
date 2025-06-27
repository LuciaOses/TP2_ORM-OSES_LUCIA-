

using System.ComponentModel.DataAnnotations;

namespace Application.Request
{
    public class ProjectCreate
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public decimal Amount { get; set; }

        public int Duration { get; set; }

        [Required]
        public int Area { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        public int User { get; set; }
    }
}
