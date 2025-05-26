using Application.Interfaces.IProjectProporsal;
using Application.Mappers;
using Application.Response;
using Application.UseCases;

namespace Application.UseCases
{
    public class GetProjectById
    {
        private readonly IProjectProposalRepository _repository;

        public GetProjectById(IProjectProposalRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProjectDetailResponse?> ExecuteAsync(Guid id)
        {
            var proposal = await _repository.GetByIdWithStepsAsync(id);
            if (proposal == null) return null;

            return ProjectMapper.ToDetailResponse(proposal);
        }
    }
}
