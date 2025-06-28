using Application.Interfaces.IArea;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Command
{
    public class AreaQuery(AprobacionDbContext context) : IAreaQuery
    {
        private readonly AprobacionDbContext _context = context;

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
