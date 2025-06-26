using Application.Response;
using Domain.Entities;


namespace Application.Mappers
{
    namespace Application.Mappers
    {
        public static class ProjectProposalMapper
        {
            public static ProjectShort ToDetail(ProjectProposal proposal)
            {
                return new ProjectShort
                {
                    Id = proposal.Id,
                    Title = proposal.Title,
                    Description = proposal.Description,
                    Area = proposal.AreaNavigation?.Name,
                    Type = proposal.TypeNavigation?.Name,
                    Amount = proposal.EstimatedAmount,
                    Duration = proposal.EstimatedDuration,
                    Status = proposal.StatusNavigation?.Name,
                };
            }
        }
    }
}
