using Domain.Entities;
using Application.Response;
using Application.Interfaces.IArea;


namespace Application.UseCases
{
    public class AreaService : IAreaService
    {
        private readonly IAreaCommands _command;
        private readonly IAreaQuery _query;

        public AreaService(IAreaCommands command, IAreaQuery query)
        {
            _command = command;
            _query = query;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _command.DeleteArea(id);
            return true;
        }

        public async Task<List<AreaResponse>> GetAllAsync()
        {
            var areas = await _query.GetListAreas();
            return areas.Select(a => new AreaResponse { Id = a.Id, Name = a.Name }).ToList();
        }

        public async Task<AreaResponse> GetByIdAsync(int id)
        {
            var area = await _query.GetAreaById(id);
            return new AreaResponse { Id = area.Id, Name = area.Name };
        }

        private Task<AreaResponse> CreateAreaResponse(Area area)
        {
            AreaResponse response = new AreaResponse
            {
                Id = area.Id,
                Name = area.Name
            };
            return Task.FromResult(response);
        }
    }

}
