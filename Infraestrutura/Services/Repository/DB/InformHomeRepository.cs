using WebAPI_Apollo.Model;
using WebAPI_Apollo.Model.ViewModel;

namespace WebAPI_Apollo.Infraestrutura.Services.Repository.DB
{
    public class InformHomeRepository : IInformHomeRepository
    {
        private readonly AppDbContext _context = new();

        public void Add(InformHome informHome)
        {
            _context.InformHome.Add(informHome);
            _context.SaveChanges();
        }

        public InformHome? JaExiste(InformHome informHome)
        {
            return _context.InformHome.FirstOrDefault(h => h.IdUsuario == informHome.IdUsuario);
        }

        public InformHome? Get(int id)
        {
            return _context.InformHome.FirstOrDefault(h => h.Id == id);
        }

        public InformHome? GetViaUsr(Guid idUsuario)
        {
            return _context.InformHome.FirstOrDefault(h => h.IdUsuario == idUsuario);
        }

        public InformHome? GetLast()
        {
            return _context.InformHome.OrderByDescending(h => h.Id).FirstOrDefault();
        }

        public void Update(InformHome informHome)
        {
            _context.InformHome.Update(informHome);
            _context.SaveChanges();
        }

        public void Delete(InformHome informHome)
        {
            _context.InformHome.Remove(informHome);
            _context.SaveChanges();
        }
    }
}
