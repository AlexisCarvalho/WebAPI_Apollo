using WebAPI_Apollo.Model;
using WebAPI_Apollo.Model.ViewModel;

namespace WebAPI_Apollo.Infraestrutura.Services.Repository.DB
{
    public class EstatisticasRepository : IEstatisticasRepository
    {
        private readonly AppDbContext _context = new AppDbContext();

        public void Add(Estatisticas estatisticas)
        {
            _context.Estatisticas.Add(estatisticas);
            _context.SaveChanges();
        }

        public Estatisticas? Get(int id)
        {
            return _context.Estatisticas.Find(id);
        }

        public Estatisticas? GetLast()
        {
            return _context.Estatisticas.OrderByDescending(e => e.Id).FirstOrDefault();
        }

        public void Update(Estatisticas estatisticas)
        {
            _context.Estatisticas.Update(estatisticas);
            _context.SaveChanges();
        }
    }
}
