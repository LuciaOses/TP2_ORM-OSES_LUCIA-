using Domain.Entities;
using Application.Response;

namespace Application.Mappers
{
    public static class StepMapper
    {
        public static ApprovalStepResponse ToShortResponse(ProjectApprovalStep step)
        {
            return new ApprovalStepResponse
            {
                Id = step.Id,
                StepOrder = step.StepOrder,
                DecisionDate = step.DecisionDate,
                Observations = step.Observations,
                ApproverRole = RoleMapper.ToDto(step.ApproverRole),
                Status = ApprovalStatusMapper.ToDto(step.StatusNavigation),
                ApproverUser = UserMapper.ToDto(step.ApproverUser)
            };
        }

        public static List<ApprovalStepResponse> ToShortResponseList(List<ProjectApprovalStep> steps)
        {
            return steps.Select(ToShortResponse).ToList();
        }
    }
}