using WebAPI_Apollo.Model.DTOs;
using WebAPI_Apollo.Model.Interacoes;
using WebAPI_Apollo.Model.ViewModel;

namespace WebAPI_Apollo.Infraestrutura.Services.Repository.RAM
{
    public class AmizadeRepositoryRAM : IAmizadeRepository
    {
        public void Add(Amizade amizade)
        {
            // Código pra substituir o autoIncrement do Banco
            var ultimaAmizade = GetLast();

            if (ultimaAmizade != null)
            {
                amizade.Id = ultimaAmizade.Id + 1;
            }
            else
            {
                amizade.Id = 1;
            }
            // Visa manter o uso de id int ao invés de trocar pra Guid

            VolatileContext.Amizades.Add(amizade);
        }

        public Amizade? JaEAmigo(Amizade amizade)
        {
            return VolatileContext.Amizades
                .FirstOrDefault(amizadesNoBanco =>
                amizadesNoBanco.Remetente == amizade.Remetente &&
                amizadesNoBanco.Destinatario == amizade.Destinatario);
        }

        public Amizade? Get(int id)
        {
            return VolatileContext.Amizades.FirstOrDefault(e => e.Id == id);
        }

        public List<Amizade> GetAllUsr(Guid idUsuario)
        {
            return VolatileContext.Amizades
                .OrderByDescending(amz => amz.Id)
                .Select(amz => new Amizade(amz.Remetente, amz.Destinatario))
                .Where(amz => amz.Destinatario == idUsuario || amz.Remetente == idUsuario)
                .ToList();
        }

        public Amizade? GetLast()
        {
            return VolatileContext.Amizades.OrderByDescending(e => e.Id).FirstOrDefault();
        }

        public void Update(Amizade amizade)
        {
            var index = VolatileContext.Amizades.FindIndex(e => e.Id == amizade.Id);
            if (index != -1)
            {
                VolatileContext.Amizades[index] = amizade;
            }
        }

        public void Delete(Amizade amizade)
        {
            VolatileContext.Amizades.Remove(amizade);
        }
    }
}
