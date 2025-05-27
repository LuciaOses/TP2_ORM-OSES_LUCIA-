using Application.Interfaces.IProjectType;
using Domain.Entities;

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
