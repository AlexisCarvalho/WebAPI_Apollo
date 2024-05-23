using WebAPI_Apollo.Model;
using WebAPI_Apollo.Model.Interacoes;
using WebAPI_Apollo.Model.ViewModel;

namespace WebAPI_Apollo.Infraestrutura.Services.Repository.RAM
{
    public class CurtidaRepositoryRAM : ICurtidaRepository
    {
        public void Add(Curtida curtida, ref Post postCurtido)
        {
            // Código pra substituir o autoIncrement do Banco
            var ultimaCurtida = GetLast();

            if (ultimaCurtida != null)
            {
                curtida.Id = ultimaCurtida.Id + 1;
            }
            else
            {
                curtida.Id = 1;
            }
            // Visa manter o uso de id int ao invés de trocar pra Guid

            postCurtido.NumCurtidas++;
            VolatileContext.Curtidas.Add(curtida);
        }

        public Curtida? JaCurtiu(Curtida curtida)
        {
            return VolatileContext.Curtidas
                .FirstOrDefault(curtidaNoBanco =>
                curtidaNoBanco.Remetente == curtida.Remetente &&
                curtidaNoBanco.Destinatario == curtida.Destinatario);
        }

        public Curtida? Get(int id)
        {
            return VolatileContext.Curtidas.FirstOrDefault(e => e.Id == id);
        }

        public Curtida? GetLast()
        {
            return VolatileContext.Curtidas.OrderByDescending(e => e.Id).FirstOrDefault();
        }

        public void Update(Curtida curtida)
        {
            var index = VolatileContext.Curtidas.FindIndex(e => e.Id == curtida.Id);
            if (index != -1)
            {
                VolatileContext.Curtidas[index] = curtida;
            }
        }

        public void Delete(Curtida curtida, ref Post postDescurtido)
        {
            postDescurtido.NumCurtidas--;
            VolatileContext.Curtidas.Remove(curtida);
        }
    }
}
