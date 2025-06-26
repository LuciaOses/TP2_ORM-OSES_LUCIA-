using Application.Response;
using Domain.Entities;

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
                Amount = proposal.EstimatedAmount,
                Duration = proposal.EstimatedDuration,

                Area = new GenericResponse
                {
                    Id = proposal.AreaNavigation.Id,
                    Name = proposal.AreaNavigation.Name
                },

                Type =  new GenericResponse
                {
                    Id = proposal.TypeNavigation.Id,
                    Name = proposal.TypeNavigation.Name
                },

                Status = new GenericResponse
                {
                    Id = proposal.StatusNavigation.Id,
                    Name = proposal.StatusNavigation.Name
                },

                
                User = new Users
                {
                    Id = proposal.CreateByNavigation.Id,
                    Name = proposal.CreateByNavigation.Name,
                    Email = proposal.CreateByNavigation.Email,
                    Role =  new GenericResponse
                    {
                        Id = proposal.CreateByNavigation.Role,
                        Name = proposal.CreateByNavigation.Name
                    }
                },

                Steps = proposal.ApprovalSteps?
                    .OrderBy(s => s.StepOrder)
                    .Select(step => new ApprovalStep
                    {
                        Id = (int)step.Id,
                        StepOrder = step.StepOrder,
                        DecisionDate = step.DecisionDate,
                        Observations = step.Observations ?? "",

                        ApproverUser = new Users
                        {
                            Id = step.ApproverUser.Id,
                            Name = step.ApproverUser.Name,
                            Email = step.ApproverUser.Email,
                            Role = new GenericResponse
                            {
                                Id = step.ApproverUser.Role,
                                Name = step.ApproverUser.Name
                            }
                        },

                        ApproverRole = new GenericResponse
                        {
                            Id = step.ApproverRole.Id,
                            Name = step.ApproverRole.Name
                        },

                        Status = new GenericResponse
                        {
                            Id = step.StatusNavigation.Id,
                            Name = step.StatusNavigation.Name
                        }
                    })
                    .ToList()
            };
        }
    }
}
