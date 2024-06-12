using WebAPI_Apollo.Domain.Model.Interacoes;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Repository.RAM
{
    public class CurtidaRepositoryRAM : ICurtidaRepository
    {
        public async Task Add(Curtida curtida)
        {
            await Task.Run(() =>
            {
                // Código pra substituir o autoIncrement do Banco
                var ultimaCurtida = GetLast().Result;

                if (ultimaCurtida != null)
                {
                    curtida.Id = ultimaCurtida.Id + 1;
                }
                else
                {
                    curtida.Id = 1;
                }
                // Visa manter o uso de id int ao invés de trocar pra Guid

                VolatileContext.Curtidas.Add(curtida);
            });
        }

        public Task<Curtida?> JaCurtiu(Curtida curtida)
        {
            var resultado = VolatileContext.Curtidas
                .FirstOrDefault(curtidaNoBanco =>
                curtidaNoBanco.Remetente == curtida.Remetente &&
                curtidaNoBanco.Destinatario == curtida.Destinatario);

            return Task.FromResult(resultado);
        }

        public Task<Curtida?> Get(int id)
        {
            var resultado = VolatileContext.Curtidas.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(resultado);
        }

        public Task<Curtida?> GetLast()
        {
            var resultado = VolatileContext.Curtidas
                .OrderByDescending(e => e.Id)
                .FirstOrDefault();

            return Task.FromResult(resultado);
        }

        public async Task Update(Curtida curtida)
        {
            await Task.Run(() =>
            {
                var index = VolatileContext.Curtidas
                    .FindIndex(e => e.Id == curtida.Id);
                if (index != -1)
                {
                    VolatileContext.Curtidas[index] = curtida;
                }
            });
        }

        public async Task Delete(Curtida curtida)
        {
            await Task.Run(() =>
            {
                VolatileContext.Curtidas.Remove(curtida);
            });
        }
    }
}
