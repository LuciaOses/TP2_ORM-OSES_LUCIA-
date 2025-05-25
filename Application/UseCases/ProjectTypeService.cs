using Application.Interfaces.IProjectType;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases
{
    public class ProjectTypeService : IProjectTypeService
    {
        private readonly IProjectTypeQuery _query;

        public ProjectTypeService(IProjectTypeQuery query)
        {
            _query = query;
        }

        public async Task<List<ProjectType>> GetAllAsync()
        {
            return await _query.GetAllProjectTypes();
        }
    }
}
