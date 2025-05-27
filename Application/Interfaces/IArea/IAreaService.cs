using Application.Response;

namespace Application.Interfaces.IArea
{
    public interface IAreaService
    {
        Task<List<GenericResponse>> GetAllAsync();
        Task<GenericResponse> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
