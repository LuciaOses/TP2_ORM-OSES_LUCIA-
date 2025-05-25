/*using Application.Interfaces;
using Application.Interfaces.IProjectProporsal;

namespace Application.UseCases
{
    public class ProjectApprovalStepService(
        IProjectApprovalStepRepository stepRepo,
        IProjectProposalRepository proposalRepo,
        ICurrentUser currentUserService,
        IBusinessRulesValidator businessRulesValidator,
        IUseCaseValidator validator)
    {
        public async Task AprobarPasoAsync()
        {
            try
            {
                Console.Write("ID del paso: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("El ID ingresado no es válido. Por favor, ingrese un número entero.");
                    return;
                }

                var paso = await stepRepo.GetByIdAsync(id);
                if (paso == null)
                {
                    Console.WriteLine($"El paso con ID {id} no se encontró en el sistema. Por favor, asegúrese de que el ID sea correcto.");
                    return;
                }

                var propuesta = await proposalRepo.GetByIdWithEstadoAsync(paso.ProjectProposalId);
                if (propuesta == null)
                {
                    Console.WriteLine("Proyecto no encontrado.");
                    return;
                }

                if (propuesta.Status == 3 || propuesta.Status == 2)
                {
                    Console.WriteLine("La propuesta ya está en estado rechazado o aprobado. No se puede modificar.");
                    return;
                }

                businessRulesValidator.ValidateProposalStatus(propuesta);
                businessRulesValidator.ValidateNoRejectedSteps(propuesta.ApprovalSteps);

                var pasoActual = propuesta.ApprovalSteps
                    .OrderBy(p => p.StepOrder)
                    .FirstOrDefault(p => p.Status == 1); // Pendiente

                if (pasoActual == null || pasoActual.Id != paso.Id)
                {
                    Console.WriteLine($"No puede aprobar el paso {paso.StepOrder} porque el paso anterior aún no ha sido aprobado. Complete el paso {pasoActual?.StepOrder} para continuar.");
                    return;
                }

                var rolId = currentUserService.GetCurrentRoleId();
                validator.ValidateUserPermissions(rolId, paso.ApproverRoleId);

                paso.Status = 2; // Aprobado
                paso.DecisionDate = DateTime.Now;
                paso.ApproverUserId = rolId;

                await stepRepo.UpdateAsync(paso);

                var hayMasPasos = propuesta.ApprovalSteps.Any(p => p.Status == 1);
                if (!hayMasPasos)
                {
                    propuesta.Status = 2; // Propuesta aprobada
                    await proposalRepo.UpdateAsync(propuesta);
                }

                await stepRepo.SaveChangesAsync();
                Console.WriteLine($"Paso aprobado correctamente. ID del paso: {paso.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al aprobar el paso: {ex.Message}");
            }
        }

        public async Task RechazarPasoAsync()
        {
            try
            {
                Console.Write("ID del paso: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("El ID ingresado no es válido. Por favor, ingrese un número entero.");
                    return;
                }

                var paso = await stepRepo.GetByIdAsync(id);
                if (paso == null)
                {
                    Console.WriteLine($"El paso con ID {id} no se encontró en el sistema. Por favor, asegúrese de que el ID sea correcto.");
                    return;
                }

                var propuesta = await proposalRepo.GetByIdWithEstadoAsync(paso.ProjectProposalId);
                if (propuesta == null)
                {
                    Console.WriteLine("Proyecto no encontrado.");
                    return;
                }

                var userRoleId = currentUserService.GetCurrentRoleId();
                validator.ValidateUserPermissions(userRoleId, paso.ApproverRoleId);

                paso.Status = 3; // Rechazado
                propuesta.Status = 3; // Proyecto rechazado

                await stepRepo.UpdateAsync(paso);
                await proposalRepo.UpdateAsync(propuesta);
                await stepRepo.SaveChangesAsync();

                Console.WriteLine("Paso rechazado. Proyecto marcado como Rechazado.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al rechazar el paso: {ex.Message}");
            }
        }

        public async Task ObservarPasoAsync()
        {
            try
            {
                Console.Write("ID del paso: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("El ID ingresado no es válido. Por favor, ingrese un número entero.");
                    return;
                }

                var paso = await stepRepo.GetByIdAsync(id);
                if (paso == null)
                {
                    Console.WriteLine($"El paso con ID {id} no se encontró en el sistema. Por favor, asegúrese de que el ID sea correcto.");
                    return;
                }

                var propuesta = await proposalRepo.GetByIdWithEstadoAsync(paso.ProjectProposalId);
                if (propuesta == null)
                {
                    Console.WriteLine("Proyecto no encontrado.");
                    return;
                }

                var userRoleId = currentUserService.GetCurrentRoleId();
                validator.ValidateUserPermissions(userRoleId, paso.ApproverRoleId);

                paso.Status = 4; // Observado
                propuesta.Status = 4; // En edición

                await stepRepo.UpdateAsync(paso);
                await proposalRepo.UpdateAsync(propuesta);
                await stepRepo.SaveChangesAsync();

                Console.WriteLine("Paso observado. Proyecto devuelto a edición.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al observar el paso: {ex.Message}");
            }
        }
        public async Task MostrarHistorialCompletoAsync(Guid proyectoId)
        {
            var propuesta = await proposalRepo.GetByIdWithEstadoAsync(proyectoId);
            if (propuesta == null)
            {
                Console.WriteLine("Proyecto no encontrado.");
                return;
            }

            var historial = propuesta.ApprovalSteps
                .OrderBy(p => p.StepOrder)
                .ToList();

            if (historial.Count == 0)
            {
                Console.WriteLine("No hay historial disponible para esta propuesta.");
                return;
            }

            Console.WriteLine($"Historial de la propuesta: {propuesta.Title}");
            foreach (var paso in historial)
            {
                string estado = paso.Status switch
                {
                    1 => "Pendiente",
                    2 => "Aprobado",
                    3 => "Rechazado",
                    4 => "Observado",
                    _ => "Desconocido"
                };

                Console.WriteLine($"Paso {paso.StepOrder} (ID: {paso.Id}): {estado}");
                Console.WriteLine($" - Rol aprobador: {paso.ApproverRoleId}");
                Console.WriteLine($" - Usuario aprobador: {paso.ApproverUserId ?? 0}");
                Console.WriteLine($" - Fecha de decisión: {paso.DecisionDate?.ToString("yyyy-MM-dd HH:mm") ?? "No se tomó una decisión sobre la propuesta"}");
                if (!string.IsNullOrWhiteSpace(paso.Observations))
                {
                    Console.WriteLine($" - Observaciones: {paso.Observations}");
                }
                Console.WriteLine(new string('-', 50));
            }
        }
    }
}*/

