using Application.Response;
using Domain.Entities;

namespace Application.Mappers
{
    public static class ProjectProposalDetailMapper
    {
        public static ProjectProposalResponseDetail ToDetailResponse(ProjectProposal proposal)
        {
            return new ProjectProposalResponseDetail
            {
                Id = proposal.Id,
                Title = proposal.Title,
                Description = proposal.Description,
                Area = AreaMapper.ToDto(proposal.AreaNavigation),
                Type = ProjectTypeMapper.ToDto(proposal.TypeNavigation),
                Amount = (double)proposal.EstimatedAmount,
                Duration = proposal.EstimatedDuration,
                Status = ApprovalStatusMapper.ToDto(proposal.StatusNavigation),
                User = UserMapper.ToDto(proposal.CreateByNavigation),
                Steps = proposal.ApprovalSteps != null
                ? proposal.ApprovalSteps
                .Select(StepMapper.ToShortResponse)
                .OrderBy(s => s.StepOrder)
                .ToList()
                : new List<ApprovalStepResponse>()
            };
        }

        public static List<ProjectProposalResponseDetail> ToDetailResponseList(List<ProjectProposal> projects)
        {
            return projects.Select(ToDetailResponse).ToList();
        }
    }
}
