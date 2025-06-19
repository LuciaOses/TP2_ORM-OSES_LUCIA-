using Application.Interfaces;
using Application.Interfaces.IUser; 
using Domain.Entities;

namespace Application.UseCases
{
    public class ApprovalService
    {
        private readonly IApprovalRuleQuery _ruleRepo;
        private readonly IProjectApprovalStepCommand _stepRepo;
        private readonly IUserCommand _userSelector;

        public ApprovalService(IApprovalRuleQuery ruleRepo, IProjectApprovalStepCommand stepRepo, IUserCommand userSelector)
        {
            _ruleRepo = ruleRepo;
            _stepRepo = stepRepo;
            _userSelector = userSelector;
        }

        public async Task GenerarWorkflowAsync(ProjectProposal propuesta)
        {
            var monto = propuesta.EstimatedAmount;
            var areaId = propuesta.Area;
            var tipoProyectoId = propuesta.Type;

            var reglasPorMonto = await _ruleRepo.GetRulesByAmountAsync(monto);

            var reglasEspecificas = reglasPorMonto
                .Where(r =>
                    (r.Area == null || r.Area == areaId) &&
                    (r.Type == null || r.Type == tipoProyectoId))
                .ToList();

            var reglasSeleccionadas = reglasEspecificas
                .GroupBy(r => r.StepOrder)
                .Select(g => g
                    .OrderByDescending(r =>
                        (r.Area.HasValue ? 1 : 0) + (r.Type.HasValue ? 1 : 0))
                    .First())
                .OrderBy(r => r.StepOrder)
                .ToList();

            foreach (var regla in reglasSeleccionadas)
            {
                var usuarios = await _userSelector.GetByRoleAsync(regla.ApproverRoleId);
                var usuario = usuarios.FirstOrDefault();

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
        }
    }
}