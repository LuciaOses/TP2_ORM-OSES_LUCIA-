using Application.Response;

namespace Application.Interfaces.IArea
{
    public interface IAreaService
    {
        Task<List<AreaResponse>> GetAllAsync();
        Task<AreaResponse> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
