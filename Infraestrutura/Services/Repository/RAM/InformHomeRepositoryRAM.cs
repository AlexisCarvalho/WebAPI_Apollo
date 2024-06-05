using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interface;

namespace WebAPI_Apollo.Infraestrutura.Services.Repository.RAM
{
    public class InformHomeRepositoryRAM : IInformHomeRepository
    {
        public void Add(InformHome informHome)
        {
            // Código pra substituir o autoIncrement do Banco
            var ultimaHome = GetLast();

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
        }

        public InformHome? JaExiste(InformHome informHome)
        {
            return VolatileContext.InformHome
                .FirstOrDefault(informHomesNoBanco =>
                    informHomesNoBanco.IdUsuario == informHome.IdUsuario);
        }

        public InformHome? Get(int id)
        {
            return VolatileContext.InformHome
                .FirstOrDefault(e => e.Id == id);
        }

        public InformHome? GetViaUsr(Guid idUsuario)
        {
            return VolatileContext.InformHome
                .FirstOrDefault(e => e.IdUsuario == idUsuario);
        }

        public InformHome? GetLast()
        {
            return VolatileContext.InformHome
                .OrderByDescending(e => e.Id)
                .FirstOrDefault();
        }

        public void Update(InformHome informHome)
        {
            var index = VolatileContext.InformHome
                .FindIndex(e => e.Id == informHome.Id);
            if (index != -1)
            {
                VolatileContext.InformHome[index] = informHome;
            }
        }

        public void Delete(InformHome informHome)
        {
            VolatileContext.InformHome.Remove(informHome);
        }
    }
}
