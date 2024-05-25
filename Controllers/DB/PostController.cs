﻿using Microsoft.AspNetCore.Mvc;
using WebAPI_Apollo.Model;
using WebAPI_Apollo.Model.DTOs;
using WebAPI_Apollo.Model.Interacoes;
using WebAPI_Apollo.Model.ViewModel;

namespace WebAPI_Apollo.Controllers.DB
{
    [ApiController]
    [Route("Post")]
    public class PostController : ControllerBase
    {
        private readonly IUsuarioRepository _usrRepository;
        private readonly IPostRepository _pstRepository;
        private readonly IMensagemRepository _msgRepository;
        private readonly ICurtidaRepository _intRepository;

        public PostController(IUsuarioRepository usrRepository, IPostRepository pstRepository, IMensagemRepository msgRepository, ICurtidaRepository intRepository)
        {
            _usrRepository = usrRepository ?? throw new ArgumentNullException();
            _pstRepository = pstRepository ?? throw new ArgumentNullException();
            _msgRepository = msgRepository ?? throw new ArgumentNullException();
            _intRepository = intRepository ?? throw new ArgumentNullException();
        }

        // Posts:

        // Adicionar um Post Padrão da Rede, sem imagem
        //[Authorize]
        [HttpPost]
        [Route("{idUsuario}/{titulo}/{descricao}")]
        public IActionResult AddPostPadrao(Guid idUsuario, string titulo, string descricao)
        {
            var usuario = _usrRepository.Get(idUsuario);

            if (usuario is null)
            {
                return NotFound("Usuario não Encontrado");
            }

            var novoPost = new Post(idUsuario, titulo, descricao);

            _pstRepository.Add(novoPost);

            var resposta = new PostPadraoDto(novoPost.Id, novoPost.IdUsuario, novoPost.Titulo, novoPost.Descricao, novoPost.NumCurtidas, novoPost.NumComentarios, novoPost.TimeStamp);
            return Ok(resposta);
        }

        // Retornar todos os Posts
        //[Authorize]
        [HttpGet]
        public IActionResult GetAllPost()
        {
            var posts = _pstRepository.GetAll();

            if (posts is null || posts.Count == 0)
            {
                return NotFound();
            }

            return Ok(posts);
        }

        // Retornar Post
        //[Authorize]
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetPost(Guid id)
        {
            var post = _pstRepository.Get(id);

            if (post is null)
            {
                return NotFound();
            }

            return Ok(new PostCompletoDto(post.Id, post.IdUsuario, post.Titulo, post.Descricao, post.CaminhoImagem, post.NumCurtidas, post.NumComentarios, post.TimeStamp));
        }

        // Atualizar informações por id
        // Isso pra editar o post já publicado (deixar pra mexer no final)
        //[Authorize]
        [HttpPut]
        [Route("{id}/{titulo}/{descricao}/{caminhoImagem}")]
        public IActionResult AtualizaPost(Guid id, string titulo, string descricao, string caminhoImagem)
        {
            var post = _pstRepository.Get(id);

            if (post is null)
            {
                return NotFound();
            }

            post.Titulo = titulo;
            post.Descricao = descricao;
            post.CaminhoImagem = caminhoImagem; // Alterar pra imagem nova

            _pstRepository.Update(post);

            return Ok(new PostCompletoDto(post.Id, post.IdUsuario, post.Titulo, post.Descricao, post.CaminhoImagem, post.NumCurtidas, post.NumComentarios, post.TimeStamp));
        }

        // Deletar Post por ID
        //[Authorize]
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(Guid id)
        {
            var post = _pstRepository.Get(id);

            if (post is null)
            {
                return NotFound();
            }

            _pstRepository.Delete(post);

            return NoContent();
        }

        // Posts/Comentarios:

        // Adicionar um comentario a um Post
        //[Authorize]
        [HttpPost]
        [Route("Comentarios/{id}/{usuarioQueComentou}/{comentario}")]
        public IActionResult AddComentarioPost(Guid id, Guid usuarioQueComentou, string comentario)
        {
            var postComentado = _pstRepository.Get(id);

            if (postComentado is null)
            {
                return NotFound("Post não Encontrado");
            }

            var usuario = _usrRepository.Get(usuarioQueComentou);

            if (usuario is null)
            {
                return NotFound("Usuario não Encontrado");
            }

            var novaMensagem = new Mensagem(usuarioQueComentou, id, comentario);

            _msgRepository.Add(novaMensagem);

            postComentado.NumComentarios++;

            _pstRepository.Update(postComentado);

            var resposta = new ChatDto(novaMensagem.Id, novaMensagem.Remetente, novaMensagem.Destinatario, novaMensagem.Conteudo, novaMensagem.TimeStamp);
            return Ok(resposta);
        }

        // Retornar Comentarios do Post
        //[Authorize]
        [HttpGet]
        [Route("Comentarios/{id}")]
        public IActionResult GetComentariosPost(Guid id)
        {
            var comentarios = _msgRepository.RecebidasPor(id);

            if (comentarios is null || comentarios.Count == 0)
            {
                return NotFound();
            }

            return Ok(comentarios);
        }

        // Posts/Interagir:
        // Adicionar uma curtida a um post
        //[Authorize]
        [HttpPost]
        [Route("Interagir/Curtir/{idPost}/{idUsuario}")]
        public IActionResult AddCurtidaPost(Guid idPost, Guid idUsuario)
        {
            var postCurtido = _pstRepository.Get(idPost);

            if (postCurtido is null)
            {
                return NotFound();
            }

            var usuario = _usrRepository.Get(idUsuario);

            if (usuario is null)
            {
                return NotFound("Usuario não Encontrado");
            }

            Curtida curtida = new Curtida(idUsuario, idPost);

            var jaCurtiu = _intRepository.JaCurtiu(curtida);

            if (jaCurtiu is null)
            {
                _intRepository.Add(curtida, ref postCurtido);
                _pstRepository.Update(postCurtido);

                var resposta = new PostPadraoDto(postCurtido.Id, postCurtido.IdUsuario, postCurtido.Titulo, postCurtido.Descricao, postCurtido.NumCurtidas, postCurtido.NumComentarios, postCurtido.TimeStamp);
                return Ok(resposta);
            }
            else
            {
                return NotFound("Post Já foi Curtido Por Esse Usuario Antes");
            }
        }

        // Adicionar uma curtida a um post
        //[Authorize]
        [HttpPost]
        [Route("Interagir/Descurtir/{idPost}/{idUsuario}")]
        public IActionResult RemoverCurtidaPost(Guid idPost, Guid idUsuario)
        {
            var postDescurtido = _pstRepository.Get(idPost);

            if (postDescurtido is null)
            {
                return NotFound();
            }

            var usuario = _usrRepository.Get(idUsuario);

            if (usuario is null)
            {
                return NotFound("Usuario não Encontrado");
            }

            Curtida descurtida = new Curtida(idUsuario, idPost);

            var curtiuAntes = _intRepository.JaCurtiu(descurtida);

            if (curtiuAntes is null)
            {
                return NotFound("Post Nunca foi Curtido Por Esse Usuario Antes");
            }

            _intRepository.Delete(curtiuAntes, ref postDescurtido);
            _pstRepository.Update(postDescurtido);

            var resposta = new PostPadraoDto(postDescurtido.Id, postDescurtido.IdUsuario, postDescurtido.Titulo, postDescurtido.Descricao, postDescurtido.NumCurtidas, postDescurtido.NumComentarios, postDescurtido.TimeStamp);
            return Ok(resposta);
        }

        // Posts/Pesquisa
        // Retornar somente posts que correspondem a determinada pesquisa
        //[Authorize]
        [HttpGet]
        [Route("Pesquisa/{termo}")]
        public IActionResult GetPostsDoUsuario(string termo)
        {
            var postsCorrespondentes = _pstRepository.GetPostsPesquisa(termo);

            if (postsCorrespondentes is null)
            {
                return NotFound("Nenhum Post Correspondente Encontrado");
            }

            return Ok(postsCorrespondentes);
        }

        // Posts/Usuario:
        // Retornar somente posts feitos por aquele usuario
        //[Authorize]
        [HttpGet]
        [Route("Usuario/{id}")]
        public IActionResult GetPostsDoUsuario(Guid id)
        {
            var usuario = _usrRepository.Get(id);

            if (usuario is null)
            {
                return NotFound("Usuario não Encontrado");
            }

            var posts = _pstRepository.PostadosPor(id);

            if (posts is null || posts.Count == 0)
            {
                return NotFound("Nenhum Post Feito Ainda...");
            }

            return Ok(posts);
        }
    }
}
