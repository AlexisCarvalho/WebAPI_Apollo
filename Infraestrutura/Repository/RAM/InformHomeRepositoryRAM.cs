using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Repository.RAM
{
    public class InformHomeRepositoryRAM : IInformHomeRepository
    {
        public async Task Add(InformHome informHome)
        {
            await Task.Run(() =>
            {
                // Código pra substituir o autoIncrement do Banco
                var ultimaHome = GetLast().Result;

                if (ultimaHome != null)
                {
                    informHome.Id = ultimaHome.Id + 1;
                }
                else
                {
                    informHome.Id = 1;
                }
                // Visa manter o uso de id int ao invés de trocar pra Guid

                VolatileContext.InformHome.Add(informHome);
            });
        }

        public Task<InformHome?> JaExiste(InformHome informHome)
        {
            var resultado = VolatileContext.InformHome
                .FirstOrDefault(informHomesNoBanco =>
                    informHomesNoBanco.IdUsuario == informHome.IdUsuario);

            return Task.FromResult(resultado);
        }

        public Task<InformHome?> Get(int id)
        {
            var resultado = VolatileContext.InformHome
                .FirstOrDefault(e => e.Id == id);

            return Task.FromResult(resultado);
        }

        public Task<InformHome?> GetViaUsr(Guid idUsuario)
        {
            var resultado = VolatileContext.InformHome
                .FirstOrDefault(e => e.IdUsuario == idUsuario);

            return Task.FromResult(resultado);
        }

        public Task<InformHome?> GetLast()
        {
            var resultado = VolatileContext.InformHome
                .OrderByDescending(e => e.Id)
                .FirstOrDefault();

            return Task.FromResult(resultado);
        }

        public async Task Update(InformHome informHome)
        {
            await Task.Run(() =>
            {
                var index = VolatileContext.InformHome
                .FindIndex(e => e.Id == informHome.Id);
                if (index != -1)
                {
                    VolatileContext.InformHome[index] = informHome;
                }
            });
        }

        public async Task Delete(InformHome informHome)
        {
            await Task.Run(() =>
            {
                VolatileContext.InformHome.Remove(informHome);
            });
        }
    }
}
