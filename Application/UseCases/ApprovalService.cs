using Application.Interfaces;
using Domain.Entities;

namespace Application.UseCases
{
    public class ApprovalService(IApprovalRuleRepository ruleRepo, IProjectApprovalStepRepository stepRepo)
    {
        private readonly IApprovalRuleRepository _ruleRepo = ruleRepo;
        private readonly IProjectApprovalStepRepository _stepRepo = stepRepo;

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
                    .First()
                ).OrderBy(r => r.StepOrder)
                .ToList();

            foreach (var regla in reglasSeleccionadas)
            {
                var paso = new ProjectApprovalStep
                {
                    ProjectProposalId = propuesta.Id,
                    StepOrder = regla.StepOrder,
                    ApproverRoleId = regla.ApproverRoleId,
                    Status = 1, // Por ejemplo: 1 = Pendiente
                };

                await _stepRepo.AddAsync(paso);
            }

            await _stepRepo.SaveChangesAsync();


            var primerPaso = reglasSeleccionadas.OrderBy(r => r.StepOrder).First();
            Console.WriteLine($"Se generó un flujo de aprobación con {reglasSeleccionadas.Count} pasos.");
            Console.WriteLine($"El primer paso está asignado al rol con ID {primerPaso.ApproverRoleId}.");
        }
    }
}

