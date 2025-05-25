using Domain.Entities;


namespace Application.Interfaces.IProjectType
{
    public interface IProjectTypeService
    {
        Task<List<ProjectType>> GetAllAsync();
    }
}
