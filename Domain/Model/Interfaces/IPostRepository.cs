using WebAPI_Apollo.Domain.DTOs;
using WebAPI_Apollo.Domain.Model.Interacoes;

namespace WebAPI_Apollo.Domain.Model.Interfaces
{
    public interface IPostRepository
    {
        void Add(Post post);
        void Delete(Post post);
        Post? Get(Guid id);
        List<PostCompletoDto> GetAll();
        List<Amizade> GetAllAmz(Guid idUsuario);
        List<PostCompletoDto> GetFeedUsr(Guid idUsuario);
        Post? GetLast();
        List<PostCompletoDto> GetPostsPesquisa(string pesquisa);
        List<PostCompletoDto> PostadosPor(Guid idUsuario);
        void Update(Post post);
    }
}