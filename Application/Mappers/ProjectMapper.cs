using Application.Response;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Application.Mappers
{
    public static class ProjectMapper
    {
        public static Project ToDetailResponse(ProjectProposal proposal)
        {
            return new Project
            {
                Id = proposal.Id,
                Title = proposal.Title,
                Description = proposal.Description,
                Area = AreaMapper.ToDto(proposal.AreaNavigation),
                Type = ProjectTypeMapper.ToDto(proposal.TypeNavigation),
                Amount = proposal.EstimatedAmount,
                Duration = proposal.EstimatedDuration,
                Status = ApprovalStatusMapper.ToDto(proposal.StatusNavigation),
                User = UserMapper.ToDto(proposal.CreateByNavigation)
            };
        }

        public static List<Project> ToResponseList(List<ProjectProposal> proposals)
        {
            return proposals.Select(ToDetailResponse).ToList();
        }
    }
}

