using Application.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public static class ProjectMapper
    {
        public static ProjectDetailResponse ToDetailResponse(ProjectProposal proposal)
        {
            return new ProjectDetailResponse
            {
                Id = proposal.Id,
                Title = proposal.Title,
                Description = proposal.Description,
                EstimatedAmount = proposal.EstimatedAmount,
                EstimatedDuration = proposal.EstimatedDuration,
                Area = proposal.AreaNavigation?.Name,
                Type = proposal.TypeNavigation?.Name,
                Status = proposal.StatusNavigation?.Name,
                CreatedBy = proposal.CreateByNavigation?.Name,
                CreateAt = proposal.CreateAt,
                ApprovalSteps = proposal.ApprovalSteps
                    .OrderBy(s => s.StepOrder)
                    .Select(step => new ProjectApprovalStepResponse
                    {
                        StepOrder = step.StepOrder,
                        ApproverUser = step.ApproverUser?.Name,
                        ApproverRole = step.ApproverRole?.Name,
                        Status = step.StatusNavigation?.Name,
                        DecisionDate = step.DecisionDate,
                        Observations = step.Observations
                    })
                    .ToList()
            };
        }
    }
}
