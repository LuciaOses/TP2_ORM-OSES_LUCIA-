
using System;
using System.Collections.Generic;

namespace Application.Response
{
    public class Project
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public decimal Amount { get; set; }
        public int Duration { get; set; }

        public required GenericResponse Status { get; set; }
        public required GenericResponse Area { get; set; }
        public required GenericResponse Type { get; set; }
        
        public required Users User { get; set; }
        public List<ApprovalStep> Steps { get; set; }
    }
}
