using WebAPI_Apollo.Model;
using WebAPI_Apollo.Model.DTOs;
using WebAPI_Apollo.Model.ViewModel;

namespace WebAPI_Apollo.Infraestrutura.Services.Repository.DB
{
    public class PostRepository : IPostsRepository
    {
        private readonly AppDbContext _context = new AppDbContext();

        public void Add(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public Post? Get(Guid id)
        {
            return _context.Posts.FirstOrDefault(post => post.Id == id);
        }

        public void Update(Post post)
        {
            _context.Posts.Update(post);
            _context.SaveChanges();
        }

        public List<PostCompletoDto> GetAll()
        {
            return _context.Posts
                .OrderByDescending(post => post.TimeStamp)
                .Select(post => new PostCompletoDto(post.Id, post.IdUsuario, post.Titulo, post.Descricao, post.CaminhoImagem, post.NumCurtidas, post.NumComentarios, post.TimeStamp))
                .ToList();
        }

        public void Delete(Post post)
        {
            _context.Posts.Remove(post);
            _context.SaveChanges();
        }

        public Post? GetLast()
        {
            return _context.Posts.OrderByDescending(e => e.Id).FirstOrDefault();
        }

        public List<PostCompletoDto> PostadosPor(Guid idUsuario)
        {
            return _context.Posts.Where(post => post.IdUsuario == idUsuario)
                                  .Select(post =>
                                      new PostCompletoDto(post.Id, post.IdUsuario, post.Titulo, post.Descricao, post.CaminhoImagem, post.NumCurtidas, post.NumComentarios, post.TimeStamp))
                                  .ToList();
        }
    }
}
