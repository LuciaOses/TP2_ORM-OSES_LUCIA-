using Application.Interfaces;
using Application.Interfaces.IUser;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.UseCases
{
    public class ApprovalService
    {
        private readonly IApprovalRuleQuery _ruleRepo;
        private readonly IProjectApprovalStepCommand _stepRepo;
        private readonly IUserCommand _userSelector;
        private readonly ILogger<ApprovalService> _logger;

        public ApprovalService(
            IApprovalRuleQuery ruleRepo,
            IProjectApprovalStepCommand stepRepo,
            IUserCommand userSelector,
            ILogger<ApprovalService> logger)
        {
            _ruleRepo = ruleRepo;
            _stepRepo = stepRepo;
            _userSelector = userSelector;
            _logger = logger;
        }

        public async Task GenerarWorkflowAsync(ProjectProposal propuesta)
        {
            try
            {
                var reglasSeleccionadas = await ObtenerReglasAprobacionAsync(propuesta);

                foreach (var regla in reglasSeleccionadas)
                {
                    var usuario = (await _userSelector.GetByRoleAsync(regla.ApproverRoleId)).FirstOrDefault();

                    if (usuario == null)
                    {
                        _logger.LogWarning("No se encontró usuario para el rol {RoleId} en paso {StepOrder}.", regla.ApproverRoleId, regla.StepOrder);
                    }

                    var paso = new ProjectApprovalStep
                    {
                        ProjectProposalId = propuesta.Id,
                        StepOrder = regla.StepOrder,
                        ApproverRoleId = regla.ApproverRoleId,
                        ApproverUserId = usuario?.Id,
                        Status = 1 // Pendiente
                    };

                    await _stepRepo.AddAsync(paso);
                }

                await _stepRepo.SaveChangesAsync();

                _logger.LogInformation("Workflow de aprobación generado correctamente para el proyecto {ProjectId}", propuesta.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar workflow para el proyecto {ProjectId}", propuesta.Id);
                throw;
            }
        }

        private async Task<List<ApprovalRule>> ObtenerReglasAprobacionAsync(ProjectProposal propuesta)
        {
            var reglasPorMonto = await _ruleRepo.GetRulesByAmountAsync(propuesta.EstimatedAmount);

            var reglasFiltradas = reglasPorMonto
                .Where(r =>
                    (r.Area == null || r.Area == propuesta.Area) &&
                    (r.Type == null || r.Type == propuesta.Type))
                .ToList();

            return reglasFiltradas
                .GroupBy(r => r.StepOrder)
                .Select(grupo =>
                    grupo.OrderByDescending(r =>
                        (r.Area.HasValue ? 1 : 0) + (r.Type.HasValue ? 1 : 0)).First())
                .OrderBy(r => r.StepOrder)
                .ToList();
        }
    }
}