using Application.Interfaces.IProjectType;
using Domain.Entities;

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
