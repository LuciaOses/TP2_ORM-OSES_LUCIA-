using Domain.Entities;


namespace Application.Interfaces.IProjectType
{
    public interface IProjectTypeQuery
    {
        Task<List<ProjectType>> GetAllProjectTypes();
    }
}
