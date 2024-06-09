using Microsoft.EntityFrameworkCore;
using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Repository.DB
{
    public class InformHomeRepository : IInformHomeRepository
    {
        private readonly AppDbContext _context = new();

        public async Task Add(InformHome informHome)
        {
            await _context.InformHome.AddAsync(informHome);
            await _context.SaveChangesAsync();
        }

        public async Task<InformHome?> Get(int id)
        {
            return await _context.InformHome.FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<InformHome?> GetLast()
        {
            return await _context.InformHome
                .OrderByDescending(h => h.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<InformHome?> GetViaUsr(Guid idUsuario)
        {
            return await _context.InformHome
                .FirstOrDefaultAsync(h => h.IdUsuario == idUsuario);
        }


        public async Task Update(InformHome informHome)
        {
            _context.InformHome.Update(informHome);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(InformHome informHome)
        {
            _context.InformHome.Remove(informHome);
            await _context.SaveChangesAsync();
        }

        public async Task<InformHome?> JaExiste(InformHome informHome)
        {
            return await _context.InformHome
                .FirstOrDefaultAsync(h => h.IdUsuario == informHome.IdUsuario);
        }
    }
}
