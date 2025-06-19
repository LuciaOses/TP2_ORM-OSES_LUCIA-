using Application.Exceptions;
using Application.Interfaces.IProjectProporsal;
using Application.Interfaces.IValidator;
using Application.Mappers.Application.Mappers;
using Application.Request;
using Application.Response;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Application.UseCases
{
    public class ProjectProposalService : IProjectProposalService
    {
        private readonly IProjectProposalCommand _repository;
        private readonly IDatabaseValidator _validator;
        private readonly ApprovalService _approvalService;
        private readonly ILogger<ProjectProposalService> _logger;

        public ProjectProposalService(
            IProjectProposalCommand repository,
            IDatabaseValidator validator,
            ApprovalService approvalService,
            ILogger<ProjectProposalService> logger)
        {
            _repository = repository;
            _validator = validator;
            _approvalService = approvalService;
            _logger = logger;
        }

        public async Task<bool> ExistingProject(string title)
        {
            return await _repository.ExistsByTitle(title);
        }

        public async Task<Project> CreateProjectProposal(
            string title,
            string? description,
            int areaId,
            int typeId,
            decimal amount,
            int duration,
            int userId)
        {
            await _validator.ValidateAreaExistsAsync(areaId);
            await _validator.ValidateUserExistsAsync(userId);

            if (!await _validator.ProjectTypeExists(typeId))
                throw new ValidationException("El tipo de proyecto especificado no existe.");

            var entity = new ProjectProposal
            {
                Id = Guid.NewGuid(),
                Title = title.Trim(),
                Description = description?.Trim(),
                Area = areaId,
                Type = typeId,
                EstimatedAmount = amount,
                EstimatedDuration = duration,
                CreateBy = userId,
                CreateAt = DateTime.UtcNow,
                Status = 1 // Pendiente
            };

            try
            {
                await _repository.AddAsync(entity);
                await _approvalService.GenerarWorkflowAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear la propuesta de proyecto: {ex.Message}");
                throw new Exception("Error al crear la propuesta de proyecto.", ex);
            }

            _logger.LogInformation($"Proyecto creado con éxito: {entity.Id}");
            return ProjectProposalMapper.ToDetail(entity);
        }

        public async Task<IEnumerable<Project>> SearchProjects(ProjectFilterRequest filters)
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

        public async Task<Project> TakeDecision(Guid projectId, DecisionStep request)
        {
            await _validator.ValidateUserExistsAsync(request.User);

            var project = await _repository.GetProjectWithStepsByIdAsync(projectId);
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

            _logger.LogInformation($"Se ha tomado una decisión sobre el proyecto: {projectId} - Estado actualizado a {project.Status}");
            return ProjectProposalMapper.ToDetail(project);
        }
    }
}