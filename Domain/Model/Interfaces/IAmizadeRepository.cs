using WebAPI_Apollo.Domain.Model.Interacoes;

namespace WebAPI_Apollo.Domain.Model.Interfaces
{
    public interface IAmizadeRepository
    {
        Task Add(Amizade amizade);
        Task<Amizade?> Get(int id);
        Task<List<Amizade>> GetAllUsr(Guid idUsuario);
        Task<Amizade?> GetLast();
        Task Update(Amizade amizade);
        Task Delete(Amizade amizade);
        Task DeletarReferencias(Guid idUsuario);
        Task<Amizade?> VerificarAmizade(Amizade amizade);
    }
}