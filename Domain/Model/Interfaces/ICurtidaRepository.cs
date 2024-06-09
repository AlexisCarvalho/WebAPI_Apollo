using WebAPI_Apollo.Domain.Model.Interacoes;

namespace WebAPI_Apollo.Domain.Model.Interfaces
{
    public interface ICurtidaRepository
    {
        Task Add(Curtida curtida);
        Task<Curtida?> Get(int id);
        Task<Curtida?> GetLast();
        Task Update(Curtida curtida);
        Task Delete(Curtida curtida);
        Task<Curtida?> JaCurtiu(Curtida curtida);
    }
}