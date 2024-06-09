using Microsoft.EntityFrameworkCore;
using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Repository.DB
{
    public class EstatisticasRepository : IEstatisticasRepository
    {
        private readonly AppDbContext _context = new();

        public async Task Add(Estatisticas estatisticas)
        {
            await _context.Estatisticas.AddAsync(estatisticas);
            await _context.SaveChangesAsync();
        }

        public async Task<Estatisticas?> Get(int id)
        {
            return await _context.Estatisticas.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Estatisticas?> GetLast()
        {
            return await _context.Estatisticas
                .OrderByDescending(e => e.Id)
                .FirstOrDefaultAsync();
        }

        public async Task Update(Estatisticas estatisticas)
        {
            _context.Estatisticas.Update(estatisticas);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Estatisticas estatisticas)
        {
            _context.Estatisticas.Remove(estatisticas);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarReferencias(int idEstatisticas)
        {
            var estatisticasDoUsuario = await _context.Estatisticas
                .Where(est => est.Id == idEstatisticas)
                .ToListAsync();

            _context.Estatisticas.RemoveRange(estatisticasDoUsuario);
            await _context.SaveChangesAsync();
        }
    }
}
