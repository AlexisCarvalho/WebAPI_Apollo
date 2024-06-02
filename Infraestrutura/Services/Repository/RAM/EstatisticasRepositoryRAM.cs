using WebAPI_Apollo.Model;
using WebAPI_Apollo.Model.ViewModel;

namespace WebAPI_Apollo.Infraestrutura.Services.Repository.RAM
{
    public class EstatisticasRepositoryRAM : IEstatisticasRepository
    {
        public void Add(Estatisticas estatisticas)
        {
            // Código pra substituir o autoIncrement do Banco
            var ultimaEstatistica = GetLast();

            if (ultimaEstatistica != null)
            {
                estatisticas.Id = ultimaEstatistica.Id + 1;
            }
            else
            {
                estatisticas.Id = 1;
            }
            // Visa manter o uso de id int ao invés de trocar pra Guid
            VolatileContext.Estatisticas.Add(estatisticas);
        }

        // De certo modo, redundante, por uso de chave primaria no banco,
        // esta aqui pois na RAM podem haver problemas com isso e ser usado
        // no futuro pra resolve-los
        public void DeletarReferencias(int idEstatisticas)
        {
            var estatisticasDoUsuario = VolatileContext.Estatisticas
                .Select(est => new Estatisticas
                (
                    est.Id,
                    est.IMC,
                    est.AguaDiaria
                ))
                .Where(amz => amz.Id == idEstatisticas)
                .ToList();

            foreach (var estatisticas in estatisticasDoUsuario)
            {
                VolatileContext.Estatisticas.Remove(estatisticas);
            }
        }

        public void Delete(Estatisticas est)
        {
            VolatileContext.Estatisticas.Remove(est);
        }

        public Estatisticas? Get(int id)
        {
            return VolatileContext.Estatisticas.FirstOrDefault(e => e.Id == id);
        }

        public Estatisticas? GetLast()
        {
            return VolatileContext.Estatisticas
                .OrderByDescending(e => e.Id)
                .FirstOrDefault();
        }

        public void Update(Estatisticas estatisticas)
        {
            var index = VolatileContext.Estatisticas
                .FindIndex(e => e.Id == estatisticas.Id);
            if (index != -1)
            {
                VolatileContext.Estatisticas[index] = estatisticas;
            }
        }
    }
}
