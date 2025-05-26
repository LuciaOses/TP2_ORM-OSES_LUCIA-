using Application.Response;
using Domain.Entities;


namespace Application.Mappers
{
    namespace Application.Mappers
    {
        public static class ProjectProposalMapper
        {
            public static ProjectProposalResponseDetail ToDetail(ProjectProposal proposal)
            {
                return new ProjectProposalResponseDetail
                {
                    Id = proposal.Id,
                    Title = proposal.Title,
                    Description = proposal.Description,
                    Area = proposal.AreaNavigation?.Name,
                    Type = proposal.TypeNavigation?.Name,
                    EstimatedAmount = proposal.EstimatedAmount,
                    EstimatedDuration = proposal.EstimatedDuration,
                    Status = proposal.StatusNavigation?.Name,
                    CreateDate = proposal.CreateAt,
                    CreatedBy = proposal.CreateByNavigation?.Name,
                };
            }
        }
    }
}
