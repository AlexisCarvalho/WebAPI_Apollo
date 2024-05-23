using WebAPI_Apollo.Model;
using WebAPI_Apollo.Model.DTOs;
using WebAPI_Apollo.Model.ViewModel;

namespace WebAPI_Apollo.Infraestrutura.Services.Repository.RAM
{
    public class PostRepositoryRAM : IPostsRepository
    {
        public void Add(Post post)
        {
            VolatileContext.Posts.Add(post);
        }

        public Post? Get(Guid id)
        {
            return VolatileContext.Posts.FirstOrDefault(post => post.Id == id);
        }

        public void Update(Post post)
        {
            var index = VolatileContext.Posts.FindIndex(m => m.Id == post.Id);
            if (index != -1)
            {
                VolatileContext.Posts[index] = post;
            }
        }

        public List<PostCompletoDto> GetAll()
        {
            return VolatileContext.Posts
                .OrderByDescending(post => post.TimeStamp)
                .Select(post => new PostCompletoDto(post.Id, post.IdUsuario, post.Titulo, post.Descricao, post.CaminhoImagem, post.NumCurtidas, post.NumComentarios, post.TimeStamp))
                .ToList();
        }

        public void Delete(Post post)
        {
            VolatileContext.Posts.Remove(post);
        }

        public Post? GetLast()
        {
            return VolatileContext.Posts.OrderByDescending(e => e.Id).FirstOrDefault();
        }

        public List<PostCompletoDto> PostadosPor(Guid idUsuario)
        {
            return VolatileContext.Posts.Where(post => post.IdUsuario == idUsuario)
                                        .Select(post =>
            new PostCompletoDto(post.Id, post.IdUsuario, post.Titulo, post.Descricao, post.CaminhoImagem, post.NumCurtidas, post.NumComentarios, post.TimeStamp))
                                        .ToList();
        }
    }
}
