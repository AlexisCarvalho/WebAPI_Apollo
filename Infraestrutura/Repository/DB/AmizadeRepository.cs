using Microsoft.EntityFrameworkCore;
using WebAPI_Apollo.Domain.Model.Interacoes;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Repository.DB
{
    public class AmizadeRepository : IAmizadeRepository
    {
        private readonly AppDbContext _context = new();

        public async Task Add(Amizade amizade)
        {
            await _context.Amizades.AddAsync(amizade);
            await _context.SaveChangesAsync();
        }

        public async Task<Amizade?> Get(int id)
        {
            return await _context.Amizades.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<Amizade>> GetAllUsr(Guid idUsuario)
        {
            return await _context.Amizades
                .Where(amz => amz.Destinatario == idUsuario
                              || amz.Remetente == idUsuario)
                .OrderByDescending(amz => amz.Id)
                .ToListAsync();
        }

        public async Task<Amizade?> GetLast()
        {
            return await _context.Amizades
                .OrderByDescending(e => e.Id)
                .FirstOrDefaultAsync();
        }

        public async Task Update(Amizade amizade)
        {
            _context.Amizades.Update(amizade);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Amizade amizade)
        {
            _context.Amizades.Remove(amizade);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarReferencias(Guid idUsuario)
        {
            var amizadesDoUsr = await _context.Amizades
                .Where(amz => amz.Destinatario == idUsuario
                              || amz.Remetente == idUsuario)
                .ToListAsync();

            foreach (var amizade in amizadesDoUsr)
            {
                if (amizade.Remetente == idUsuario)
                {
                    var homeAmigo = await _context.InformHome
                        .FirstOrDefaultAsync(h => h.IdUsuario == amizade.Destinatario);

                    if (homeAmigo != null)
                    {
                        homeAmigo.NumAmigos--;
                        _context.InformHome.Update(homeAmigo);
                    }
                }
                else
                {
                    var homeAmigo = await _context.InformHome
                        .FirstOrDefaultAsync(h => h.IdUsuario == amizade.Remetente);

                    if (homeAmigo != null)
                    {
                        homeAmigo.NumAmigos--;
                        _context.InformHome.Update(homeAmigo);
                    }
                }
            }
            _context.Amizades.RemoveRange(amizadesDoUsr);
            await _context.SaveChangesAsync();
        }

        public async Task<Amizade?> VerificarAmizade(Amizade amizade)
        {
            return await _context.Amizades
                .FirstOrDefaultAsync(amz => amz.Remetente == amizade.Remetente
                                        && amz.Destinatario == amizade.Destinatario
                                        || amz.Destinatario == amizade.Remetente
                                        && amz.Remetente == amizade.Destinatario);
        }
    }
}
