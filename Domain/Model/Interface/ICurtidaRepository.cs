using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interacoes;

namespace WebAPI_Apollo.Domain.Model.Interface
{
    public interface ICurtidaRepository
    {
        void Add(Curtida curtida, ref Post postCurtido);
        void Delete(Curtida curtida, ref Post postDescurtido);
        Curtida? Get(int id);
        Curtida? GetLast();
        Curtida? JaCurtiu(Curtida curtida);
        void Update(Curtida curtida);
    }
}