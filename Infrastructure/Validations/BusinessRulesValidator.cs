using Domain.Entities;
using Application.Interfaces.IValidator;

namespace Infraestructura.Validations
{
    public class BusinessRulesValidator : IBusinessRulesValidator
    {
        public void ValidateProposalStatus(ProjectProposal proposal)
        {
            if (proposal.Status == 3) // 3 = Rechazado
            {
                throw new Exception("La solicitud ya ha sido rechazada y no puede ser modificada.");
            }

            if (proposal.Status == 2) // 2 = Aprobado
            {
                throw new Exception("La solicitud ya ha sido aprobada y no puede ser modificada.");
            }
        }

        public void ValidateNoRejectedSteps(IEnumerable<ProjectApprovalStep> steps)
        {
            if (steps.Any(s => s.Status == 3))
            {
                throw new Exception("Existen pasos rechazados, no se puede continuar.");
            }
        }
    }
}