using WebAPI_Apollo.Model;
using WebAPI_Apollo.Model.ViewModel;

namespace WebAPI_Apollo.Infraestrutura.Services.Repository.DB
{
    public class EstatisticasRepository : IEstatisticasRepository
    {
        private readonly AppDbContext _context = new();

        public void Add(Estatisticas estatisticas)
        {
            _context.Estatisticas.Add(estatisticas);
            _context.SaveChanges();
        }

        public void DeletarReferencias(int idEstatisticas)
        {
            var estatisticasDoUsuario = _context.Estatisticas
                .Where(est => est.Id == idEstatisticas)
                .ToList();

            _context.Estatisticas.RemoveRange(estatisticasDoUsuario);
            _context.SaveChanges();
        }

        public void Delete(Estatisticas estatisticas)
        {
            _context.Estatisticas.Remove(estatisticas);
            _context.SaveChanges();
        }

        public Estatisticas? Get(int id)
        {
            return _context.Estatisticas.FirstOrDefault(e => e.Id == id);
        }

        public Estatisticas? GetLast()
        {
            return _context.Estatisticas
                .OrderByDescending(e => e.Id)
                .FirstOrDefault();
        }

        public void Update(Estatisticas estatisticas)
        {
            _context.Estatisticas.Update(estatisticas);
            _context.SaveChanges();
        }
    }
}
