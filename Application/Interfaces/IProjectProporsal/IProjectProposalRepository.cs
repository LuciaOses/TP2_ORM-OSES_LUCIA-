using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IProjectProporsal
{
    public interface IProjectProposalRepository
    {
        Task<bool> ExistsByTitle(string title);
        Task AddAsync(ProjectProposal proposal);
        IQueryable<ProjectProposal> Query();
        Task<ProjectProposal?> GetByIdWithStepsAsync(Guid id);
        Task UpdateAsync(ProjectProposal proposal);
        Task SaveChangesAsync();
        
    }
}
