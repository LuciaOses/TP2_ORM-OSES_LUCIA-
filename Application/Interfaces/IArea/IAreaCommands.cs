using Domain.Entities;


namespace Application.Interfaces.IArea
{
    public interface IAreaCommands
    {
        Task<Area> InsertArea(Area area);
        Task DeleteArea(int id);
    }
}
