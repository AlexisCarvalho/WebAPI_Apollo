using WebAPI_Apollo.Domain.Model.Interacoes;

namespace WebAPI_Apollo.Domain.Model.Interfaces
{
    public interface ICurtidaRepository
    {
        void Add(Curtida curtida);
        void Delete(Curtida curtida);
        Curtida? Get(int id);
        Curtida? GetLast();
        Curtida? JaCurtiu(Curtida curtida);
        void Update(Curtida curtida);
    }
}