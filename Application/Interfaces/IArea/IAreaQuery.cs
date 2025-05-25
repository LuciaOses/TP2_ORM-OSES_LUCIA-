using Domain.Entities;

namespace Application.Interfaces.IArea
{
    public interface IAreaQuery
    {
        Task<Area> GetAreaById(int id);
        Task<List<Area>> GetListAreas();
    }
}
