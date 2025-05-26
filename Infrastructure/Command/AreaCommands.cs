using Application.Exceptions;
using Application.Interfaces.IArea;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Command
{
    public class AreaCommands(AprobacionDbContext context, IAreaQuery query) : IAreaCommands
    {
        private readonly AprobacionDbContext _context = context;
        private readonly IAreaQuery _query = query;

        public async Task<Area> InsertArea(Area area)
        {
            try
            {
                _context.Areas.Add(area);
                await _context.SaveChangesAsync();
                return area;
            }
            catch (DbUpdateException)
            {
                throw new ExceptionBadRequest("Error al insertar en la base de datos");
            }
        }

        public async Task DeleteArea(int id)
        {
            try
            {
                var area = await _query.GetAreaById(id);
                _context.Areas.Remove(area);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new ExceptionBadRequest("Error al eliminar en la base de datos");
            }
        }
    }
}
