using WebAPI_Apollo.Model;
using WebAPI_Apollo.Model.Interacoes;
using WebAPI_Apollo.Model.ViewModel;

namespace WebAPI_Apollo.Infraestrutura.Services.Repository.DB
{
    public class AmizadeRepository : IAmizadeRepository
    {
        private readonly AppDbContext _context = new();

        public void Add(Amizade amizade)
        {
            _context.Amizades.Add(amizade);
            _context.SaveChanges();
        }

        public Amizade? VerificarAmizade(Amizade amizade)
        {
            return _context.Amizades
                .FirstOrDefault(amz => (amz.Remetente == amizade.Remetente 
                                        && amz.Destinatario == amizade.Destinatario) 
                                        || (amz.Destinatario == amizade.Remetente 
                                        && amz.Remetente == amizade.Destinatario));
        }

        public Amizade? Get(int id)
        {
            return _context.Amizades.FirstOrDefault(e => e.Id == id);
        }

        public List<Amizade> GetAllUsr(Guid idUsuario)
        {
            return _context.Amizades
                .Where(amz => amz.Destinatario == idUsuario 
                              || amz.Remetente == idUsuario)
                .OrderByDescending(amz => amz.Id)
                .ToList();
        }

        public Amizade? GetLast()
        {
            return _context.Amizades
                .OrderByDescending(e => e.Id)
                .FirstOrDefault();
        }

        public void Update(Amizade amizade)
        {
            _context.Amizades.Update(amizade);
            _context.SaveChanges();
        }

        public void Delete(Amizade amizade)
        {
            _context.Amizades.Remove(amizade);
            _context.SaveChanges();
        }

        public void DeletarReferencias(Guid idUsuario)
        {
            var amizadesDoUsr = _context.Amizades
                .Where(amz => amz.Destinatario == idUsuario 
                              || amz.Remetente == idUsuario)
                .ToList();

            foreach (var amizade in amizadesDoUsr)
            {
                if (amizade.Remetente == idUsuario)
                {
                    var homeAmigo = _context.InformHome
                        .FirstOrDefault(h => h.IdUsuario == amizade.Destinatario);

                    if (homeAmigo != null)
                    {
                        homeAmigo.NumAmigos--;
                        _context.InformHome.Update(homeAmigo);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    var homeAmigo = _context.InformHome
                        .FirstOrDefault(h => h.IdUsuario == amizade.Remetente);

                    if (homeAmigo != null)
                    {
                        homeAmigo.NumAmigos--;
                        _context.InformHome.Update(homeAmigo);
                        _context.SaveChanges();
                    }
                }
            }
            _context.Amizades.RemoveRange(amizadesDoUsr);
            _context.SaveChanges();
        }
    }
}
