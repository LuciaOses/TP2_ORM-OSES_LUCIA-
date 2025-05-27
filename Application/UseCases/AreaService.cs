using Domain.Entities;
using Application.Response;
using Application.Interfaces.IArea;


namespace Application.UseCases
{
    public class AreaService(IAreaCommands command, IAreaQuery query) : IAreaService
    {
        private readonly IAreaCommands _command = command;
        private readonly IAreaQuery _query = query;

        public async Task<bool> DeleteAsync(int id)
        {
            await _command.DeleteArea(id);
            return true;
        }

        public async Task<List<GenericResponse>> GetAllAsync()
        {
            var areas = await _query.GetListAreas();
            return areas.Select(a => new GenericResponse { Id = a.Id, Name = a.Name }).ToList();
        }

        public async Task<GenericResponse> GetByIdAsync(int id)
        {
            var area = await _query.GetAreaById(id);
            return new GenericResponse { Id = area.Id, Name = area.Name };
        }

        private Task<GenericResponse> CreateAreaResponse(Area area)
        {
            GenericResponse response = new()
            {
                Id = area.Id,
                Name = area.Name
            };
            return Task.FromResult(response);
        }
    }

}
