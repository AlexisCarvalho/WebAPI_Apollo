using WebAPI_Apollo.Application.Services;
using WebAPI_Apollo.Domain.DTOs;
using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interacoes;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Repository.RAM
{
    public class PostRepositoryRAM : IPostRepository
    {
        public async Task Add(Post post)
        {
            await Task.Run(() =>
            {
                VolatileContext.Posts.Add(post);
            });
        }

        public Task<Post?> Get(Guid id)
        {
            var resultado = VolatileContext.Posts
                .FirstOrDefault(post => post.Id == id);

            return Task.FromResult(resultado);
        }

        public async Task Update(Post post)
        {
            await Task.Run(() =>
            {
                var index = VolatileContext.Posts.FindIndex(m => m.Id == post.Id);
                if (index != -1)
                {
                    VolatileContext.Posts[index] = post;
                }
            });
        }

        public Task<List<PostCompletoDto>> GetAll()
        {
            var resultado = VolatileContext.Posts
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
                .ToList();

            return Task.FromResult(resultado);
        }

        public Task<List<PostCompletoDto>> GetFeedUsr(Guid idUsuario)
        {
            var amizades = GetAllAmz(idUsuario).Result;
            List<Guid> idAmigos = new();
            List<PostCompletoDto> feed = new();

            foreach (var amizade in amizades)
            {
                if (amizade.Remetente == idUsuario)
                {
                    idAmigos.Add(amizade.Destinatario);
                }
                else
                {
                    idAmigos.Add(amizade.Remetente);
                }
            }

            foreach (var id in idAmigos)
            {
                feed.AddRange(PostadosPor(id).Result);
            }

            feed.Sort((postUm, postDois) =>
            postDois.timeStamp.CompareTo(postUm.timeStamp));
            // Ordem inversa para ser decrescente, trocar causo queira crescente

            return Task.FromResult(feed);
        }

        public Task<List<PostCompletoDto>> GetPostsPesquisa(string pesquisa)
        {
            var candidatos = VolatileContext.Posts
                .Where(post =>
                    post.Titulo.Contains
                        (pesquisa, StringComparison.CurrentCultureIgnoreCase) ||
                    post.Descricao.Contains
                        (pesquisa, StringComparison.CurrentCultureIgnoreCase))
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
                )).ToList();

            var resultados = candidatos
                .Where(post => AlgoritmosDePesquisa.SimilaridadeDeJaccard(post.titulo, pesquisa) > 0.4
                               || AlgoritmosDePesquisa.SimilaridadeDeJaccard(post.descricao, pesquisa) > 0.2)
                .ToList();

            return Task.FromResult(resultados);
        }

        public async Task Delete(Post post)
        {
            await Task.Run(() =>
            {
                VolatileContext.Posts.Remove(post);
            });
        }

        public Task<Post?> GetLast()
        {
            var resultado = VolatileContext.Posts
                .OrderByDescending(e => e.Id).FirstOrDefault();

            return Task.FromResult(resultado);
        }

        public Task<List<PostCompletoDto>> PostadosPor(Guid idUsuario)
        {
            var resultado = VolatileContext.Posts
                .Where(post => post.IdUsuario == idUsuario)
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
                .ToList();

            return Task.FromResult(resultado);
        }

        public Task<List<Amizade>> GetAllAmz(Guid idUsuario)
        {
            var resultado = VolatileContext.Amizades
                .OrderByDescending(amz => amz.Id)
                .Select(amz => new Amizade(amz.Remetente, amz.Destinatario))
                .Where(amz => amz.Destinatario == idUsuario || amz.Remetente == idUsuario)
                .ToList();

            return Task.FromResult(resultado);
        }
    }
}
