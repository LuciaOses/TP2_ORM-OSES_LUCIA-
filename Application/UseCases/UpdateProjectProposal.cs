using Application.Exceptions;
using Application.Interfaces.IProjectProporsal;
using Application.Mappers;
using Application.Request;
using Application.Response;

namespace Application.UseCases
{
    public class UpdateProjectProposal
    {
        private readonly IProjectProposalCommand _repository;

        public UpdateProjectProposal(IProjectProposalCommand repository)
        {
            _repository = repository;
        }

        public async Task<ProjectProposalResponseDetail?> ExecuteAsync(Guid id, ProjectUpdate request)
        {
            var proposal = await _repository.GetByIdWithStepsAsync(id)
                ?? throw new ExceptionNotFound("Proyecto no encontrado.");

            if (proposal.Status != 4)
                throw new ExceptionBadRequest("Solo se puede modificar un proyecto en estado Observado.");

            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ExceptionBadRequest("El título no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(request.Description))
                throw new ExceptionBadRequest("La descripción no puede estar vacía.");
            if (request.Duration <= 0)
                throw new ExceptionBadRequest("La duración debe ser mayor a 0.");

            var isDuplicated = await _repository.ExistsByTitleExceptIdAsync(request.Title, id);
            if (isDuplicated)
                throw new ExceptionBadRequest("Ya existe un proyecto con ese título.");

            proposal.Title = request.Title.Trim();
            proposal.Description = request.Description;
            proposal.EstimatedDuration = request.Duration;

            await _repository.UpdateAsync(proposal);
            await _repository.SaveChangesAsync();

            return ProjectProposalDetailMapper.ToDetailResponse(proposal);
        }
    }
}