using Application.Exceptions;
using Application.Interfaces.IProjectProporsal;
using Application.Interfaces.IValidator;
using Application.Mappers.Application.Mappers;
using Application.Request;
using Application.Response;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            return await _repository.ExistsByTitle(title.Trim());
        }

        public async Task<ProjectProposal> CreateProjectProposal(
            string title,
            string description,
            int areaId,
            int typeId,
            decimal amount,
            int duration,
            int userId)
        {
            await ValidateInputsAsync(title, description, areaId, typeId, amount, duration, userId);

            var newProposal = new ProjectProposal
            {
                Id = Guid.NewGuid(),
                Title = title.Trim(),
                Description = description.Trim(),
                Area = areaId,
                Type = typeId,
                EstimatedAmount = amount,
                EstimatedDuration = duration,
                CreateBy = userId,
                CreateAt = DateTime.UtcNow,
                Status = 1 // Estado inicial: Pendiente
            };

            try
            {
                await _repository.AddAsync(newProposal);
                await _repository.SaveChangesAsync();
                await _approvalService.GenerarWorkflowAsync(newProposal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la propuesta de proyecto.");
                throw new Exception("Ocurrió un error al crear la propuesta de proyecto.", ex);
            }

            var loadedProject = await _repository.GetProjectWithStepsByIdAsync(newProposal.Id);

            if (loadedProject is null)
            {
                _logger.LogError("No se pudo cargar el proyecto creado con sus relaciones.");
                throw new Exception("No se pudo recuperar el proyecto luego de su creación.");
            }

            _logger.LogInformation("Proyecto creado con éxito. ID: {ProjectId}", loadedProject.Id);
            return loadedProject;
        }

        private async Task ValidateInputsAsync(string title, string? description, int areaId, int typeId, decimal amount, int duration, int userId)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ExceptionBadRequest("El título del proyecto es obligatorio.");

            if (await ExistingProject(title))
                throw new ExceptionBadRequest($"Ya existe un proyecto con el título '{title.Trim()}'.");

            if (string.IsNullOrWhiteSpace(description))
                throw new ExceptionBadRequest("La descripción del proyecto es obligatoria.");

            if (amount <= 0)
                throw new ExceptionBadRequest("El monto estimado debe ser mayor a cero.");

            if (duration <= 0)
                throw new ExceptionBadRequest("La duración estimada debe ser mayor a cero.");

            if (!await _validator.AreaExistsAsync(areaId))
                throw new ExceptionBadRequest($"El área especificada (ID: {areaId}) no existe.");

            if (!await _validator.UserExistsAsync(userId))
                throw new ExceptionBadRequest($"El usuario especificado (ID: {userId}) no existe.");

            if (!await _validator.ProjectTypeExists(typeId))
                throw new ExceptionBadRequest($"El tipo de proyecto especificado (ID: {typeId}) no existe.");
        }

        public async Task<IEnumerable<ProjectShort>> SearchProjects(ProjectFilterRequest filters)
        {
            var query = _repository.Query().AsQueryable();

            if (filters.ApprovalUser.HasValue)
            {
                query = query.Include(p => p.ApprovalSteps)
                             .Where(p => p.ApprovalSteps.Any(s => s.ApproverUserId == filters.ApprovalUser.Value));
            }

            if (!string.IsNullOrWhiteSpace(filters.Title))
                query = query.Where(p => EF.Functions.Like(p.Title, $"%{filters.Title}%"));

            if (filters.Status.HasValue)
                query = query.Where(p => p.Status == filters.Status.Value);

            if (filters.Applicant.HasValue)
                query = query.Where(p => p.CreateBy == filters.Applicant.Value);

            var results = await query.ToListAsync();
            return results.Select(ProjectProposalMapper.ToDetail);
        }

        public async Task<ProjectShort> TakeDecision(Guid projectId, DecisionStep request)
        {
            await _validator.ValidateUserExistsAsync(request.User);

            var project = await _repository.GetProjectWithStepsByIdAsync(projectId)
                          ?? throw new ExceptionNotFound("Proyecto no encontrado.");

            var step = project.ApprovalSteps
                              .FirstOrDefault(s => s.ApproverUserId == request.User);

            if (step is null)
                throw new ExceptionBadRequest("El usuario no tiene permisos para aprobar este proyecto.");

            if (step.Status is not (1 or 4)) 
                throw new ExceptionBadRequest("El paso ya fue resuelto y no puede modificarse.");

            if ((request.Status == 3 || request.Status == 4) && string.IsNullOrWhiteSpace(request.Observation))
                throw new ExceptionBadRequest("Debe ingresar una observación al observar o rechazar.");

            step.Status = request.Status;
            step.Observations = request.Observation;
            step.DecisionDate = DateTime.UtcNow;

            
            project.Status = request.Status switch
            {
                2 when project.ApprovalSteps.All(s => s.Status == 2) => 2, // Aprobado
                3 => 3, // Rechazado
                4 => 4, // Observado
                _ => project.Status
            };

            await _repository.UpdateAsync(project);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Decisión tomada sobre el proyecto {ProjectId}. Estado actualizado a {Status}.", projectId, project.Status);
            return ProjectProposalMapper.ToDetail(project);
        }
    }
}