using WebAPI_Apollo.Domain.DTOs;
using WebAPI_Apollo.Domain.Model.Interacoes;

namespace WebAPI_Apollo.Domain.Model.Interfaces
{
    public interface IPostRepository
    {
        Task Add(Post post);
        Task<Post?> Get(Guid id);
        Task<List<PostCompletoDto>> GetAll();
        Task<List<Amizade>> GetAllAmz(Guid idUsuario);
        Task<Post?> GetLast();
        Task<List<PostCompletoDto>> GetFeedUsr(Guid idUsuario);
        Task<List<PostCompletoDto>> GetPostsPesquisa(string pesquisa);
        Task<List<PostCompletoDto>> PostadosPor(Guid idUsuario);
        Task Update(Post post);
        Task Delete(Post post);
    }
}