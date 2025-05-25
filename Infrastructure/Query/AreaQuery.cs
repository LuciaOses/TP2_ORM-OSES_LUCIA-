using Application.Interfaces.IArea;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Query
{
    public class AreaQuery : IAreaQuery
    {
        private readonly AprobacionDbContext _context;

        public AreaQuery(AprobacionDbContext context)
        {
            _context = context;
        }

        public async Task<Area> GetAreaById(int id)
        {
            return await _context.Areas.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Area>> GetListAreas()
        {
            return await _context.Areas.ToListAsync();
        }
    }
}
