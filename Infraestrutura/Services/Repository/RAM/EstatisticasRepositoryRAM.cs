using WebAPI_Apollo.Model;
using WebAPI_Apollo.Model.ViewModel;

namespace WebAPI_Apollo.Infraestrutura.Services.Repository.RAM
{
    public class EstatisticasRepositoryRAM : IEstatisticasRepository
    {
        public void Add(Estatisticas estatisticas)
        {
            VolatileContext.Estatisticas.Add(estatisticas);
        }

        public Estatisticas? Get(int id)
        {
            return VolatileContext.Estatisticas.FirstOrDefault(e => e.Id == id);
        }

        public Estatisticas? GetLast()
        {
            return VolatileContext.Estatisticas.OrderByDescending(e => e.Id).FirstOrDefault();
        }

        public void Update(Estatisticas estatisticas)
        {
            var index = VolatileContext.Estatisticas.FindIndex(e => e.Id == estatisticas.Id);
            if (index != -1)
            {
                VolatileContext.Estatisticas[index] = estatisticas;
            }
        }
    }
}
