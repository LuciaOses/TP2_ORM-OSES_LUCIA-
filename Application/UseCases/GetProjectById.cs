using Application.Interfaces.IProjectProporsal;
using Application.Mappers;
using Application.Response;
namespace Application.UseCases
{
    public class GetProjectById
    {
        private readonly IProjectProposalCommand _repository;

        public GetProjectById(IProjectProposalCommand repository)
        {
            _repository = repository;
        }

        public async Task<ProjectProposalResponseDetail?> ExecuteAsync(Guid id)
        {
            var proposal = await _repository.GetByIdWithStepsAsync(id);
            if (proposal == null) return null;

            return ProjectProposalDetailMapper.ToDetailResponse(proposal);
        }
    }
}
