using Microsoft.EntityFrameworkCore;
using WebAPI_Apollo.Domain.Model.Interacoes;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Repository.DB
{
    public class CurtidaRepository : ICurtidaRepository
    {
        private readonly AppDbContext _context = new();

        public async Task Add(Curtida curtida)
        {
            await _context.Curtidas.AddAsync(curtida);
            await _context.SaveChangesAsync();
        }

        public async Task<Curtida?> Get(int id)
        {
            return await _context.Curtidas.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Curtida?> GetLast()
        {
            return await _context.Curtidas
                .OrderByDescending(c => c.Id)
                .FirstOrDefaultAsync();
        }

        public async Task Update(Curtida curtida)
        {
            _context.Curtidas.Update(curtida);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Curtida curtida)
        {
            _context.Curtidas.Remove(curtida);
            await _context.SaveChangesAsync();
        }

        public async Task<Curtida?> JaCurtiu(Curtida curtida)
        {
            return await _context.Curtidas
                .FirstOrDefaultAsync(c => c.Remetente == curtida.Remetente
                                     && c.Destinatario == curtida.Destinatario);
        }
    }
}
