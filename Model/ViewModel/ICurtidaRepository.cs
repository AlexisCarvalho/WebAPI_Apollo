using WebAPI_Apollo.Model.Interacoes;

namespace WebAPI_Apollo.Model.ViewModel
{
    public interface ICurtidaRepository
    {
        void Add(Curtida curtida, ref Post postCurtido);
        Curtida? Get(int id);
        Curtida? GetLast();
        void Update(Curtida curtida);
        Curtida? JaCurtiu(Curtida curtida);
        void Delete(Curtida curtida, ref Post postDescurtido);
    }
}
