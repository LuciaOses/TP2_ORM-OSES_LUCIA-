/*using Application.Interfaces;
using Application.Interfaces.IProjectProporsal;
using Domain.Entities;

namespace Application.UseCases
{
    public class ProjectProposalService(
        IProjectProposalRepository repo,
        ApprovalService approvalService,
        IDatabaseValidator validator)
    {
        private readonly IProjectProposalRepository _repo = repo;
        private readonly ApprovalService _approvalService = approvalService;
        private readonly IDatabaseValidator _validator = validator;

        public async Task CrearSolicitud()
        {
            Console.Write("Título: ");
            var titulo = Console.ReadLine();

            Console.Write("Descripción: ");
            var descripcion = Console.ReadLine();

            Console.Write("Monto estimado: ");
            if (!decimal.TryParse(Console.ReadLine(), out var monto))
            {
                Console.WriteLine("Monto inválido.");
                return;
            }

            Console.Write("Duración estimada (días): ");
            if (!int.TryParse(Console.ReadLine(), out var duracion))
            {
                Console.WriteLine("Duración inválida.");
                return;
            }

            Console.WriteLine("\nÁreas disponibles:");
            Console.WriteLine("ID\tNombre");
            Console.WriteLine("1\tFinanzas");
            Console.WriteLine("2\tTecnología");
            Console.WriteLine("3\tRecursos Humanos");
            Console.WriteLine("4\tOperaciones");

            Console.Write("\nID del Área: ");
            if (!int.TryParse(Console.ReadLine(), out var areaId) || areaId < 1 || areaId > 4)
            {
                Console.WriteLine("ID de área inválido. Por favor, seleccione un ID válido de la lista.");
                return;
            }

            Console.WriteLine("\nTipos de proyecto disponibles:");
            Console.WriteLine("ID\tNombre");
            Console.WriteLine("1\tMejora de Procesos");
            Console.WriteLine("2\tInnovación y Desarrollo");
            Console.WriteLine("3\tInfraestructura");
            Console.WriteLine("4\tCapacitación Interna");

            Console.Write("\nID del Tipo de Proyecto: ");
            if (!int.TryParse(Console.ReadLine(), out var tipoId) || tipoId < 1 || tipoId > 4)
            {
                Console.WriteLine("ID de tipo de proyecto inválido. Por favor, seleccione un ID válido de la lista.");
                return;
            }

            Console.Write("ID del Usuario Creador: ");
            if (!int.TryParse(Console.ReadLine(), out var creadoPor))
            {
                Console.WriteLine("ID de usuario inválido.");
                return;
            }

      
            try
            {
                await _validator.ValidateUserExistsAsync(creadoPor);
                await _validator.ValidateAreaExistsAsync(areaId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de validación: {ex.Message}");
                return;
            }

            var solicitud = new ProjectProposal
            {
                Id = Guid.NewGuid(),
                Title = titulo!,
                Description = descripcion!,
                EstimatedAmount = monto,
                EstimatedDuration = duracion,
                Area = areaId,
                Type = tipoId,
                Status = 1, // Pendiente
                CreateAt = DateTime.Now,
                CreateBy = creadoPor
            };

            await _repo.AddAsync(solicitud);
            Console.WriteLine("Solicitud creada con éxito.");

            await _approvalService.GenerarWorkflowAsync(solicitud);
            Console.WriteLine("Flujo de aprobación generado.");
        }

        public async Task VerEstadoSolicitudAsync()
        {
            Console.Write("Ingrese el ID de la solicitud: ");
            var input = Console.ReadLine();
            if (!Guid.TryParse(input, out var id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var propuesta = await _repo.GetByIdWithEstadoAsync(id);
            if (propuesta == null)
            {
                Console.WriteLine("Solicitud no encontrada.");
                return;
            }

            Console.WriteLine($"Título: {propuesta.Title}");
            Console.WriteLine($"Estado: {propuesta.StatusNavigation?.Name ?? "Desconocido"}");
            Console.WriteLine($"Monto estimado: {propuesta.EstimatedAmount}");
            Console.WriteLine($"Área: {propuesta.Area}");
            Console.WriteLine($"Tipo de proyecto: {propuesta.Type}");
        }
        public async Task ListarSolicitudes()
        {
            var solicitudes = await _repo.GetAllAsync();

            if (solicitudes == null || solicitudes.Count == 0)
            {
                Console.WriteLine("No hay solicitudes registradas.");
                return;
            }

            Console.WriteLine("\n--- Lista de Solicitudes ---");
            foreach (var solicitud in solicitudes)
            {
                Console.WriteLine($"ID: {solicitud.Id}");
                Console.WriteLine($"Título: {solicitud.Title}");
                Console.WriteLine($"Descripción: {solicitud.Description}");
                Console.WriteLine($"Monto estimado: {solicitud.EstimatedAmount}");
                Console.WriteLine($"Duración: {solicitud.EstimatedDuration} días");
                Console.WriteLine($"Área ID: {solicitud.Area}");
                Console.WriteLine($"Tipo ID: {solicitud.Type}");
                Console.WriteLine($"Estado: {solicitud.Status}");
                Console.WriteLine($"Creado el: {solicitud.CreateAt}");
                Console.WriteLine($"Creado por (Usuario ID): {solicitud.CreateBy}");
                Console.WriteLine("-----------------------------\n");
            }
        }
    }
}*/
