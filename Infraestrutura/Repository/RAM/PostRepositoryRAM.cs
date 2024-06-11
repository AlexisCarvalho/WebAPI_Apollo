﻿using WebAPI_Apollo.Application.Services;
using WebAPI_Apollo.Domain.DTOs;
using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interacoes;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Infraestrutura.Repository.RAM
{
    public class PostRepositoryRAM : IPostRepository
    {
        public void Add(Post post)
        {
            VolatileContext.Posts.Add(post);
        }

        public Post? Get(Guid id)
        {
            return VolatileContext.Posts
                .FirstOrDefault(post => post.Id == id);
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
        }

        public List<PostCompletoDto> GetFeedUsr(Guid idUsuario)
        {
            var amizades = GetAllAmz(idUsuario);
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
                feed.AddRange(PostadosPor(id));
            }

            feed.Sort((postUm, postDois) =>
            postDois.timeStamp.CompareTo(postUm.timeStamp));
            // Ordem inversa para ser decrescente, trocar causo queira crescente

            return feed;
        }

        public List<PostCompletoDto> GetPostsPesquisa(string pesquisa)
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

            return resultados;
        }

        public void Delete(Post post)
        {
            VolatileContext.Posts.Remove(post);
        }

        public Post? GetLast()
        {
            return VolatileContext.Posts
                .OrderByDescending(e => e.Id).FirstOrDefault();
        }

        public List<PostCompletoDto> PostadosPor(Guid idUsuario)
        {
            return VolatileContext.Posts
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
        }

        public List<Amizade> GetAllAmz(Guid idUsuario)
        {
            return VolatileContext.Amizades
                .OrderByDescending(amz => amz.Id)
                .Select(amz => new Amizade(amz.Remetente, amz.Destinatario))
                .Where(amz => amz.Destinatario == idUsuario || amz.Remetente == idUsuario)
                .ToList();
        }
    }
}
