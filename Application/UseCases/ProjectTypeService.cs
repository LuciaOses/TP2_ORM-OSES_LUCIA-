using Application.Interfaces.IProjectType;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class ProjectTypeService(IProjectTypeQuery query) : IProjectTypeService
    {
        private readonly IProjectTypeQuery _query = query;

        public async Task<List<ProjectType>> GetAllAsync()
        {
            return await _query.GetAllProjectTypes();
        }
    }
}
