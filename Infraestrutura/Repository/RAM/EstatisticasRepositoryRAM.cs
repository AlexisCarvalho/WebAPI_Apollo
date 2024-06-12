using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Repository.RAM
{
    public class EstatisticasRepositoryRAM : IEstatisticasRepository
    {
        public async Task Add(Estatisticas estatisticas)
        {
            await Task.Run(() => 
            { 
                // Código pra substituir o autoIncrement do Banco
                var ultimaEstatistica = GetLast().Result;

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
            });
        }

        // De certo modo, redundante, por uso de chave primaria no banco,
        // esta aqui pois na RAM podem haver problemas com isso e ser usado
        // no futuro pra resolve-los
        public async Task DeletarReferencias(int idEstatisticas)
        {
            await Task.Run(() =>
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
            });
        }

        public async Task Delete(Estatisticas est)
        {
            await Task.Run(() =>
            {
                VolatileContext.Estatisticas.Remove(est);
            });
        }

        public Task<Estatisticas?> Get(int id)
        {
            var resultado = VolatileContext.Estatisticas
                .FirstOrDefault(e => e.Id == id);

            return Task.FromResult(resultado);
        }

        public Task<Estatisticas?> GetLast()
        {
            var resultado = VolatileContext.Estatisticas
                .OrderByDescending(e => e.Id)
                .FirstOrDefault();

            return Task.FromResult(resultado);
        }

        public async Task Update(Estatisticas estatisticas)
        {
            await Task.Run(() =>
            {
                var index = VolatileContext.Estatisticas
                .FindIndex(e => e.Id == estatisticas.Id);
                if (index != -1)
                {
                    VolatileContext.Estatisticas[index] = estatisticas;
                }
            });
        }
    }
}
