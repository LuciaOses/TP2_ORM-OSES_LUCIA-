using Application.Response;
using Domain.Entities;


namespace Application.Mappers
{
    namespace Application.Mappers
    {
        public static class ProjectShortMapper
        {
            public static ProjectShort ToDetail(ProjectProposal proposal)
            {
                return new ProjectShort
                {
                    Id = proposal.Id,
                    Title = proposal.Title,
                    Description = proposal.Description,
                    Amount = proposal.EstimatedAmount,
                    Duration = proposal.EstimatedDuration,
                    Area = proposal.AreaNavigation.Name,
                    Status = proposal.StatusNavigation.Name,
                    Type = proposal.TypeNavigation.Name
                };
            }
        }
    }
}
