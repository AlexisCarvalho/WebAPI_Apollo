using WebAPI_Apollo.Model;
using WebAPI_Apollo.Model.DTOs;
using WebAPI_Apollo.Model.Interacoes;
using WebAPI_Apollo.Model.ViewModel;

namespace WebAPI_Apollo.Infraestrutura.Services.Repository.DB
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context = new();

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

        public List<PostCompletoDto> GetFeedUsr(Guid idUsuario)
        {
            var amizades = GetAllAmz(idUsuario);

            var idAmigos = amizades.Select(amz => amz.Destinatario == idUsuario ? amz.Remetente : amz.Destinatario).ToList();

            var feed = _context.Posts
                .Where(post => idAmigos.Contains(post.IdUsuario))
                .OrderByDescending(post => post.TimeStamp)
                .Select(post => new PostCompletoDto(post.Id, post.IdUsuario, post.Titulo, post.Descricao, post.CaminhoImagem, post.NumCurtidas, post.NumComentarios, post.TimeStamp))
                .ToList();

            return feed;
        }

        public List<PostCompletoDto> GetPostsPesquisa(string pesquisa)
        {
            return _context.Posts
                .Where(post => AlgoritmosDePesquisa.SimilaridadeDeJaccard(post.Titulo, pesquisa) > 0.4
                            || AlgoritmosDePesquisa.SimilaridadeDeJaccard(post.Descricao, pesquisa) > 0.7)
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
            return _context.Posts.OrderByDescending(post => post.Id).FirstOrDefault();
        }

        public List<PostCompletoDto> PostadosPor(Guid idUsuario)
        {
            return _context.Posts
                .Where(post => post.IdUsuario == idUsuario)
                .OrderByDescending(post => post.TimeStamp)
                .Select(post => new PostCompletoDto(post.Id, post.IdUsuario, post.Titulo, post.Descricao, post.CaminhoImagem, post.NumCurtidas, post.NumComentarios, post.TimeStamp))
                .ToList();
        }

        public List<Amizade> GetAllAmz(Guid idUsuario)
        {
            return _context.Amizades
               .Where(amz => amz.Destinatario == idUsuario || amz.Remetente == idUsuario)
               .ToList();
        }
    }
}
