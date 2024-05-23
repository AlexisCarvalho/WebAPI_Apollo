using WebAPI_Apollo.Model.DTOs;

namespace WebAPI_Apollo.Model.ViewModel
{
    public interface IPostsRepository
    {
        void Add(Post post);
        List<PostCompletoDto> GetAll();
        Post? Get(Guid id);
        void Update(Post post);
        void Delete(Post post);
        List<PostCompletoDto> PostadosPor(Guid idUsuario);
    }
}
