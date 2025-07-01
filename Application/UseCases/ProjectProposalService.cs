using Application.Exceptions;
using Application.Interfaces.IProjectProporsal;
using Application.Interfaces.IValidator;
using Application.Mappers;
using Application.Mappers.Application.Mappers;
using Application.Request;
using Application.Response;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.UseCases
{
    public class ProjectProposalService(
        IProjectProposalCommand repository,
        IDatabaseValidator validator,
        ApprovalService approvalService,
        ILogger<ProjectProposalService> logger) : IProjectProposalService
    {
        private readonly IProjectProposalCommand _repository = repository;
        private readonly IDatabaseValidator _validator = validator;
        private readonly ApprovalService _approvalService = approvalService;
        private readonly ILogger<ProjectProposalService> _logger = logger;

        public async Task<bool> ExistsByTitle(string title)
        {
            return await _repository.ExistsByTitle(title.Trim());
        }

        public async Task<ProjectProposalResponseDetail> CreateProjectProposal(ProjectCreate request)
        {
            var (area, type, user) = await ValidateInputsAndLoadEntitiesAsync(
                request.Title,
                request.Description,
                request.Area,
                request.Type,
                request.Amount,
                request.Duration,
                request.User
            );

            var newProposal = new ProjectProposal
            {
                Id = Guid.NewGuid(),
                Title = request.Title.Trim(),
                Description = request.Description.Trim(),
                Area = area.Id,
                Type = type.Id,
                CreateBy = user.Id,
                EstimatedAmount = request.Amount,
                EstimatedDuration = request.Duration,
                CreateAt = DateTime.UtcNow,
                Status = 1 // Pendiente
            };

            try
            {
                await _repository.AddAsync(newProposal);
                await _repository.SaveChangesAsync();
                await _approvalService.GenerarWorkflowAsync(newProposal);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Error de base de datos al guardar la propuesta de proyecto.");
                throw new ApplicationException("Error al guardar la propuesta de proyecto en la base de datos.");
            }
            catch (Exception ex) when (ex is not ExceptionBadRequest)
            {
                _logger.LogError(ex, "Error inesperado al crear la propuesta de proyecto.");
                throw new ApplicationException("Ocurrió un error inesperado al crear la propuesta de proyecto.");
            }

            var loadedProposal = await _repository.GetByIdWithStepsAsync(newProposal.Id);

            if (loadedProposal == null)
            {
                _logger.LogError("No se pudo cargar la propuesta creada con sus relaciones. ID: {ProjectId}", newProposal.Id);
                throw new ApplicationException("No se pudo recuperar el proyecto luego de su creación.");
            }

            _logger.LogInformation("Propuesta de proyecto creada exitosamente. ID: {ProjectId}", loadedProposal.Id);

            return ProjectProposalDetailMapper.ToDetailResponse(loadedProposal);
        }
        private async Task<(Area, ProjectType, User)> ValidateInputsAndLoadEntitiesAsync(string title, string? description, int areaId, int typeId, decimal amount, int duration, int userId)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ExceptionBadRequest("El título del proyecto es obligatorio.");

            if (await ExistsByTitle(title))
                throw new ExceptionBadRequest($"Ya existe un proyecto con el título '{title.Trim()}'.");

            if (string.IsNullOrWhiteSpace(description))
                throw new ExceptionBadRequest("La descripción del proyecto es obligatoria.");

            if (amount <= 0)
                throw new ExceptionBadRequest("El monto estimado debe ser mayor a cero.");

            if (duration <= 0)
                throw new ExceptionBadRequest("La duración estimada debe ser mayor a cero.");

            var area = await _validator.GetAreaByIdAsync(areaId)
                       ?? throw new ExceptionBadRequest($"El área especificada (ID: {areaId}) no existe.");

            var type = await _validator.GetProjectTypeByIdAsync(typeId)
                       ?? throw new ExceptionBadRequest($"El tipo de proyecto especificado (ID: {typeId}) no existe.");

            var user = await _validator.GetUserById(userId)
                       ?? throw new ExceptionBadRequest($"El usuario especificado (ID: {userId}) no existe.");

            return (area, type, user);
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
            return results.Select(ProjectShortMapper.ToDetail);
        }

        public async Task<ProjectProposalResponseDetail> TakeDecision(Guid projectId, DecisionStep request)
        {
            await _validator.ValidateUserExistsAsync(request.User);

            var project = await _repository.GetByIdWithStepsAsync(projectId)
                          ?? throw new ExceptionNotFound("Proyecto no encontrado.");

            if (project.Status is not (1 or 4))
                throw new ExceptionBadRequest("Solo se puede modificar un proyecto en estado Pendiente u Observado.");

            var step = project.ApprovalSteps.FirstOrDefault(s => s.Id == request.Id)
                       ?? throw new ExceptionBadRequest("Paso no encontrado en el proyecto.");

            if (step.ApproverUserId != request.User)
                throw new ExceptionBadRequest("El usuario no tiene permisos para aprobar este paso.");

            if (request.Status is not (2 or 3 or 4))
                throw new ExceptionBadRequest("Estado inválido. Debe ser 2 (Aprobado), 3 (Rechazado) o 4 (Observado).");

            if (step.Status == 2)
                throw new ExceptionBadRequest("No se puede modificar un paso que ya fue aprobado.");
            if (step.Status == 3)
                throw new ExceptionBadRequest("No se puede modificar un paso que ya fue rechazado.");

            if (step.Status == 4 && project.Status != 4)
                throw new ExceptionBadRequest("No se puede modificar un paso observado si el proyecto no está observado.");

            if (step.Status == request.Status)
                throw new ExceptionBadRequest("El paso ya tiene este estado asignado.");

            var firstPending = project.ApprovalSteps
                .Where(s => s.Status == 1)
                .OrderBy(s => s.StepOrder)
                .FirstOrDefault();

            if (firstPending != null && step.Id != firstPending.Id)
                throw new ExceptionConflict("Solo se puede tomar decisión sobre el primer paso pendiente.");

            if ((request.Status == 3 || request.Status == 4) && string.IsNullOrWhiteSpace(request.Observation))
                throw new ExceptionBadRequest("Debe ingresar una observación al observar o rechazar.");

            
            step.Status = request.Status;
            step.Observations = request.Observation?.Trim();
            step.DecisionDate = DateTime.UtcNow;

            
            var updatedStatuses = project.ApprovalSteps
                .Select(s => s.Id == step.Id ? request.Status : s.Status)
                .ToList();

            if (updatedStatuses.Contains(3))
                project.Status = 3; 
            else if (updatedStatuses.Contains(4))
                project.Status = 4; // Observado
            else if (updatedStatuses.All(s => s == 2))
                project.Status = 2; 
            else
                project.Status = 1; // Pendiente

            await _repository.SaveChangesAsync();

            var updated = await _repository.GetByIdWithStepsAsync(projectId)
                ?? throw new ExceptionBadRequest("Error interno: no se pudo recargar el proyecto actualizado.");

            _logger.LogInformation("Decisión registrada en proyecto {ProjectId}. Nuevo estado: {Status}.", projectId, updated.Status);

            return ProjectProposalDetailMapper.ToDetailResponse(updated);
        }

    }
}