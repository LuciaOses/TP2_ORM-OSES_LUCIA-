using Application.Interfaces.IProjectProporsal;
using Application.Interfaces.IValidator;
using Application.Mappers.Application.Mappers;
using Application.Response;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using Application.Request;
using Microsoft.EntityFrameworkCore;
using Application.Exceptions;

namespace Application.UseCases
{
    public class ProjectProposalService : IProjectProposalService
    {
        private readonly IProjectProposalRepository _repository;
        private readonly IDatabaseValidator _validator;

        public ProjectProposalService(IProjectProposalRepository repository, IDatabaseValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<bool> ExistingProject(string title)
        {
            return await _repository.ExistsByTitle(title);
        }

        public async Task<ProjectProposalResponseDetail> CreateProjectProposal(string title, string? description, int areaId, int typeId, decimal amount, int duration, int userId)
        {
            await _validator.ValidateAreaExistsAsync(areaId);
            await _validator.ValidateUserExistsAsync(userId);

            if (!await _validator.ProjectTypeExists(typeId))
                throw new ValidationException("El tipo de proyecto especificado no existe.");

            var entity = new ProjectProposal
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                Area = areaId,
                Type = typeId,
                EstimatedAmount = amount,
                EstimatedDuration = duration,
                CreateBy = userId,
                CreateAt = DateTime.UtcNow,
                Status = 1 // Estado inicial
            };

            await _repository.AddAsync(entity);
            return ProjectProposalMapper.ToDetail(entity);
        }

        public async Task<IEnumerable<ProjectProposalResponseDetail>> SearchProjects(ProjectFilterRequest filters)
        {
            var query = _repository.Query().AsQueryable();

            if (filters.ApprovalUser.HasValue)
            {
                query = query.Include(p => p.ApprovalSteps)
                             .Where(p => p.ApprovalSteps.Any(s => s.ApproverUserId == filters.ApprovalUser.Value));
            }

            if (!string.IsNullOrWhiteSpace(filters.Title))
                query = query.Where(p => p.Title.Contains(filters.Title));

            if (filters.Status.HasValue)
                query = query.Where(p => p.Status == filters.Status.Value);

            if (filters.Applicant.HasValue)
                query = query.Where(p => p.CreateBy == filters.Applicant.Value);

            var result = await query.ToListAsync();
            return result.Select(ProjectProposalMapper.ToDetail);
        }

        public async Task<ProjectProposalResponseDetail> TakeDecision(Guid projectId, DecisionStepRequest request)
        {
            await _validator.ValidateUserExistsAsync(request.User);

            var project = await _repository.GetByIdWithStepsAsync(projectId);
            if (project is null)
                throw new ExceptionNotFound("Proyecto no encontrado");

            var step = project.ApprovalSteps
                              .FirstOrDefault(s => s.ApproverUserId == request.User);

            if (step is null)
                throw new ExceptionBadRequest("El usuario no está habilitado para aprobar este proyecto.");

            if (step.Status is not 1 and not 3)
                throw new ExceptionBadRequest("El paso ya fue resuelto; no puede modificarse.");

            if ((request.Status == 3 || request.Status == 4) &&
                string.IsNullOrWhiteSpace(request.Observation))
                throw new ValidationException("La observación es obligatoria para observar o rechazar.");

            step.Status = request.Status;  
            step.Observations = request.Observation;
            step.DecisionDate = DateTime.UtcNow;

            switch (request.Status)
            {
                case 2: // Aprobado
                    if (project.ApprovalSteps.All(s => s.Status == 2))
                        project.Status = 2; 
                    break;

                case 3: // Observado
                    project.Status = 3;
                    break;

                case 4: // Rechazado
                    project.Status = 4;
                    break;
            }

            await _repository.UpdateAsync(project);
            await _repository.SaveChangesAsync();

            return ProjectProposalMapper.ToDetail(project);
        }

    }
}
