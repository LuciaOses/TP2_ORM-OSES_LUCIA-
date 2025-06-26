using Domain.Entities;
using Application.Response;

namespace Application.Mappers
{
    public static class StepMapper
    {
        public static ApprovalStep ToShortResponse(ProjectApprovalStep step)
        {
            return new ApprovalStep
            {
                Id = step.Id,
                StepOrder = step.StepOrder,
                DecisionDate = step.DecisionDate,
                Observations = step.Observations,
                ApproverRole = RoleMapper.ToDto(step.ApproverRole),
                Status = ApprovalStatusMapper.ToDto(step.StatusNavigation),
                ApproverUser = UserMapper.ToDto(step.ApproverUser),
            };
        }

        
        public static List<ApprovalStep> ToShortResponseList(List<ProjectApprovalStep> steps)
        {
            return steps.Select(ToShortResponse).ToList();
        }
    }
}