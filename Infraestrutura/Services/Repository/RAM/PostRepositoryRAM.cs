using System.Collections.Generic;
using WebAPI_Apollo.Model;
using WebAPI_Apollo.Model.DTOs;
using WebAPI_Apollo.Model.Interacoes;
using WebAPI_Apollo.Model.ViewModel;

namespace WebAPI_Apollo.Infraestrutura.Services.Repository.RAM
{
    public class PostRepositoryRAM : IPostsRepository
    {
        private readonly AmizadeRepositoryRAM _amzdRepository = new();

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

        public List<PostCompletoDto> GetFeedUsr(Guid idUsuario)
        {
            var amizades = _amzdRepository.GetAllUsr(idUsuario);
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
                    idAmigos.Add(amizade.Destinatario);
                }
            }

            foreach (var id in idAmigos)
            {
                feed.AddRange(PostadosPor(id));
            }

            feed.Sort((postUm, postDois) => postDois.timeStamp.CompareTo(postUm.timeStamp)); // Ordem inversa para ser decrescente, trocar causo queira crescente

            return feed;
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
