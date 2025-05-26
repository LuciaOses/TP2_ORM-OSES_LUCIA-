using Application.Interfaces.IProjectProporsal;
using Application.Mappers;
using Application.Request;
using Application.Response;
using Domain.Entities;

namespace Application.UseCases
{
    public class UpdateProjectProposal
    {
        private readonly IProjectProposalRepository _repository;

        public UpdateProjectProposal(IProjectProposalRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProjectDetailResponse?> ExecuteAsync(Guid id, ProjectUpdateRequest request)
        {
            var proposal = await _repository.GetByIdWithStepsAsync(id);
            if (proposal == null) return null;

            if (proposal.Status != 1)
                return ProjectDetailResponse.Conflict;

            proposal.Title = request.Title.Trim();
            proposal.Description = request.Description;
            proposal.EstimatedDuration = request.Duration;

            await _repository.UpdateAsync(proposal);
            await _repository.SaveChangesAsync();

            return ProjectMapper.ToDetailResponse(proposal);
        }
    }
}
