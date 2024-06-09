using Microsoft.EntityFrameworkCore;
using WebAPI_Apollo.Application.Services;
using WebAPI_Apollo.Domain.DTOs;
using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interacoes;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Repository.DB
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context = new();

        public async Task Add(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task<Post?> Get(Guid id)
        {
            return await _context.Posts
                .FirstOrDefaultAsync(post => post.Id == id);
        }

        public async Task<List<PostCompletoDto>> GetAll()
        {
            return await _context.Posts
                .OrderByDescending(post => post.TimeStamp)
                .Select(post => new PostCompletoDto
                (
                    post.Id,
                    post.IdUsuario,
                    post.Titulo,
                    post.Descricao,
                    post.ImagemBase64,
                    post.NumCurtidas,
                    post.NumComentarios,
                    post.TimeStamp
                ))
                .ToListAsync();
        }

        public async Task<List<Amizade>> GetAllAmz(Guid idUsuario)
        {
            return await _context.Amizades
               .Where(amz => amz.Destinatario == idUsuario
                             || amz.Remetente == idUsuario)
               .ToListAsync();
        }

        public async Task<Post?> GetLast()
        {
            return await _context.Posts
                .OrderByDescending(post => post.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<PostCompletoDto>> GetFeedUsr(Guid idUsuario)
        {
            var amizades = await GetAllAmz(idUsuario);

            var idAmigos = amizades
                .Select(amz => amz.Destinatario == idUsuario ? amz.Remetente : amz.Destinatario)
                .ToList();

            var feed = await _context.Posts
                .Where(post => idAmigos.Contains(post.IdUsuario))
                .OrderByDescending(post => post.TimeStamp)
                .Select(post => new PostCompletoDto
                (
                    post.Id,
                    post.IdUsuario,
                    post.Titulo,
                    post.Descricao,
                    post.ImagemBase64,
                    post.NumCurtidas,
                    post.NumComentarios,
                    post.TimeStamp
                ))
                .ToListAsync();

            return feed;
        }

        public async Task<List<PostCompletoDto>> GetPostsPesquisa(string pesquisa)
        {
            var candidatos = await _context.Posts
                .Where(post =>
                    post.Titulo.ToLower().Contains
                        (pesquisa.ToLower()) ||
                    post.Descricao.ToLower().Contains
                        (pesquisa.ToLower()))
                .Select(post => new PostCompletoDto
                (
                    post.Id,
                    post.IdUsuario,
                    post.Titulo,
                    post.Descricao,
                    post.ImagemBase64,
                    post.NumCurtidas,
                    post.NumComentarios,
                    post.TimeStamp
                ))
                .ToListAsync();

            var resultados = candidatos
                .Where(post => AlgoritmosDePesquisa.SimilaridadeDeJaccard(post.titulo, pesquisa) > 0.4
                               || AlgoritmosDePesquisa.SimilaridadeDeJaccard(post.descricao, pesquisa) > 0.2)
                .ToList();

            return resultados;
        }


        public async Task<List<PostCompletoDto>> PostadosPor(Guid idUsuario)
        {
            return await _context.Posts
                .Where(post => post.IdUsuario == idUsuario)
                .OrderByDescending(post => post.TimeStamp)
                .Select(post => new PostCompletoDto
                (
                    post.Id,
                    post.IdUsuario,
                    post.Titulo,
                    post.Descricao,
                    post.ImagemBase64,
                    post.NumCurtidas,
                    post.NumComentarios,
                    post.TimeStamp
                ))
                .ToListAsync();
        }

        public async Task Update(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Post post)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }
}
