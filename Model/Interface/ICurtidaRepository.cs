using WebAPI_Apollo.Model.Interacoes;

namespace WebAPI_Apollo.Model.ViewModel
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