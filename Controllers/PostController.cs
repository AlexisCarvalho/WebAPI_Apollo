using Microsoft.AspNetCore.Mvc;
using WebAPI_Apollo.Domain.DTOs;
using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interacoes;
using WebAPI_Apollo.Domain.Model.Interfaces;
using WebAPI_Apollo.Infraestrutura;

namespace WebAPI_Apollo.Controllers
{
    [ApiController]
    [Route("Post")]
    public class PostController : ControllerBase
    {
        private readonly IUsuarioRepository _usrRepository;
        private readonly IPostRepository _pstRepository;
        private readonly IMensagemRepository _msgRepository;
        private readonly ICurtidaRepository _crtRepository;
        private readonly IAmizadeRepository _amzdRepository;
        private readonly INotificacaoRepository _ntfRepository;
        private readonly IInformHomeRepository _infHomeRepository;

        public PostController(IUsuarioRepository usrRepository, IPostRepository pstRepository, IMensagemRepository msgRepository, ICurtidaRepository crtRepository, IAmizadeRepository amzdRepository, INotificacaoRepository ntfRepository, IInformHomeRepository infHomeRepository)
        {
            _usrRepository = usrRepository ?? throw new ArgumentNullException();
            _pstRepository = pstRepository ?? throw new ArgumentNullException();
            _msgRepository = msgRepository ?? throw new ArgumentNullException();
            _crtRepository = crtRepository ?? throw new ArgumentNullException();
            _amzdRepository = amzdRepository ?? throw new ArgumentNullException();
            _ntfRepository = ntfRepository ?? throw new ArgumentNullException();
            _infHomeRepository = infHomeRepository ?? throw new ArgumentNullException();
        }

        // Posts:

        // Adicionar um Post Padrão da Rede, sem imagem
        [HttpPost]
        [Route("{titulo}/{descricao}")]
        public IActionResult AddPostPadrao(string titulo, string descricao)
        {
            var usuario = _usrRepository.Get(ConfigUsuario.CurrentUserId);

            if (usuario is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            var novoPost = new Post(usuario.Id, titulo, descricao);

            _pstRepository.Add(novoPost);

            var amizades = _amzdRepository.GetAllUsr(usuario.Id);

            foreach (var amizade in amizades)
            {
                if (amizade.Remetente == usuario.Id)
                {
                    var usuarioQuePostou = _usrRepository.Get(usuario.Id);
                    var amigo = _usrRepository.Get(amizade.Destinatario);
                    var homeAmigo = _infHomeRepository.GetViaUsr(amizade.Destinatario);

                    if (usuarioQuePostou is null)
                    {
                        return Problem("Seus dados não foram encontrados");
                    }

                    if (amigo is null)
                    {
                        return Problem("Um amigo da sua lista parece não existir por algum motivo");
                    }

                    if (homeAmigo is null)
                    {
                        return Problem("A Home do seu amigo parece não existir de modo inesperado");
                    }

                    var tipoNotificacao = 2;
                    var mensagem = $"{amigo.Nome.Split(" ")[0]}! Eu acabei de postar: {novoPost.Titulo}, vem dar uma olhada";

                    Notificacao ntf = new Notificacao
                        (
                            usuario.Id,
                            amizade.Destinatario,
                            tipoNotificacao,
                            mensagem
                         );

                    _ntfRepository.Add(ntf);

                    homeAmigo.NumNotificacoesNaoLidas++;
                    _infHomeRepository.Update(homeAmigo);
                }
                else
                {
                    var usuarioQuePostou = _usrRepository.Get(usuario.Id);
                    var amigo = _usrRepository.Get(amizade.Remetente);
                    var homeAmigo = _infHomeRepository.GetViaUsr(amizade.Remetente);

                    if (usuarioQuePostou is null)
                    {
                        return Problem("Seus dados não foram encontrados");
                    }

                    if (amigo is null)
                    {
                        return Problem("Um amigo da sua lista parece não existir por algum motivo");
                    }

                    if (homeAmigo is null)
                    {
                        return Problem("A Home do seu amigo parece não existir de modo inesperado");
                    }

                    var tipoNotificacao = 2;
                    var mensagem = $"{amigo.Nome.Split(" ")[0]}! Eu acabei de postar: {novoPost.Titulo}, vem dar uma olhada";

                    Notificacao ntf = new Notificacao
                        (
                            usuario.Id,
                            amizade.Remetente,
                            tipoNotificacao,
                            mensagem
                        );

                    _ntfRepository.Add(ntf);

                    homeAmigo.NumNotificacoesNaoLidas++;
                    _infHomeRepository.Update(homeAmigo);
                }
            }

            // ******* Area destinada aos ganhos de XP automatico da Rede ****** //
            Calculos c = new();

            c.GanharXP(1000, ref usuario);

            _usrRepository.Update(usuario);
            // ******* Area destinada aos ganhos de XP automatico da Rede ****** //

            var resposta = new PostPadraoDto
                (
                    novoPost.Id,
                    novoPost.IdUsuario,
                    novoPost.Titulo,
                    novoPost.Descricao,
                    novoPost.NumCurtidas,
                    novoPost.NumComentarios,
                    novoPost.TimeStamp
                );

            return Ok(resposta);
        }

        // Adicionar um Post Padrão da Rede, com imagem
        [HttpPost]
        [Route("{titulo}/{descricao}/{imagemBase64}")]
        public IActionResult AddPostImagem(string titulo, string descricao, string imagemBase64)
        {
            var usuario = _usrRepository.Get(ConfigUsuario.CurrentUserId);

            if (usuario is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            var novoPost = new Post(usuario.Id, titulo, descricao, imagemBase64);

            _pstRepository.Add(novoPost);

            var amizades = _amzdRepository.GetAllUsr(usuario.Id);

            foreach (var amizade in amizades)
            {
                if (amizade.Remetente == usuario.Id)
                {
                    var usuarioQuePostou = _usrRepository.Get(usuario.Id);
                    var amigo = _usrRepository.Get(amizade.Destinatario);
                    var homeAmigo = _infHomeRepository.GetViaUsr(amizade.Destinatario);

                    if (usuarioQuePostou is null)
                    {
                        return Problem("Seus dados não foram encontrados");
                    }

                    if (amigo is null)
                    {
                        return Problem("Um amigo da sua lista parece não existir por algum motivo");
                    }

                    if (homeAmigo is null)
                    {
                        return Problem("A Home do seu amigo parece não existir de modo inesperado");
                    }

                    var tipoNotificacao = 2;
                    var mensagem = $"{amigo.Nome.Split(" ")[0]}! Eu acabei de postar: {novoPost.Titulo}, vem dar uma olhada";

                    Notificacao ntf = new Notificacao
                        (
                            usuario.Id,
                            amizade.Destinatario,
                            tipoNotificacao,
                            mensagem
                        );

                    _ntfRepository.Add(ntf);

                    homeAmigo.NumNotificacoesNaoLidas++;
                    _infHomeRepository.Update(homeAmigo);
                }
                else
                {
                    var usuarioQuePostou = _usrRepository.Get(usuario.Id);
                    var amigo = _usrRepository.Get(amizade.Remetente);
                    var homeAmigo = _infHomeRepository.GetViaUsr(amizade.Remetente);

                    if (usuarioQuePostou is null)
                    {
                        return Problem("Seus dados não foram encontrados");
                    }

                    if (amigo is null)
                    {
                        return Problem("Um amigo da sua lista parece não existir por algum motivo");
                    }

                    if (homeAmigo is null)
                    {
                        return Problem("A Home do seu amigo parece não existir de modo inesperado");
                    }

                    var tipoNotificacao = 2;
                    var mensagem = $"{amigo.Nome.Split(" ")[0]}! Eu acabei de postar: {novoPost.Titulo}, vem dar uma olhada";

                    Notificacao ntf = new Notificacao
                        (
                            usuario.Id,
                            amizade.Remetente,
                            tipoNotificacao,
                            mensagem
                        );

                    _ntfRepository.Add(ntf);

                    homeAmigo.NumNotificacoesNaoLidas++;
                    _infHomeRepository.Update(homeAmigo);
                }
            }

            // ******* Area destinada aos ganhos de XP automatico da Rede ****** //
            Calculos c = new();

            c.GanharXP(1200, ref usuario);

            _usrRepository.Update(usuario);
            // ******* Area destinada aos ganhos de XP automatico da Rede ****** //

            var resposta = new PostPadraoDto(novoPost.Id, novoPost.IdUsuario, novoPost.Titulo, novoPost.Descricao, novoPost.NumCurtidas, novoPost.NumComentarios, novoPost.TimeStamp);
            return Ok(resposta);
        }

        // Retornar todos os Posts
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
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetPost(Guid id)
        {
            var post = _pstRepository.Get(id);

            if (post is null)
            {
                return NotFound("Post Não Encontrado");
            }

            var usuario = _usrRepository.Get(post.IdUsuario);

            if (usuario is null)
            {
                return NotFound("Usuario Que Postou Não Encontrado");
            }

            var resposta = new PostSistemaDto
                (
                    post.Id,
                    post.IdUsuario,
                    usuario.ImagemPerfil,
                    usuario.Nome,
                    post.Titulo,
                    post.Descricao,
                    post.ImagemBase64,
                    post.NumCurtidas,
                    post.NumComentarios,
                    post.TimeStamp
                );

            return Ok(resposta);
        }

        // Atualizar informações por id
        // Isso pra editar o post já publicado (deixar pra mexer no final)
        [HttpPut]
        [Route("{id}/{titulo}/{descricao}/{imagemBase64}")]
        public IActionResult AtualizaPost(Guid id, string titulo, string descricao, string imagemBase64)
        {
            var post = _pstRepository.Get(id);

            if (post is null)
            {
                return NotFound("Post Não Encontrado");
            }

            var usuario = _usrRepository.Get(post.IdUsuario);

            if (usuario is null)
            {
                return NotFound("Usuario Que Postou Não Encontrado");
            }

            post.Titulo = titulo;
            post.Descricao = descricao;
            post.ImagemBase64 = imagemBase64; // Alterar pra imagem nova

            _pstRepository.Update(post);

            var resposta = new PostSistemaDto
                (
                    post.Id,
                    post.IdUsuario,
                    usuario.ImagemPerfil,
                    usuario.Nome,
                    post.Titulo,
                    post.Descricao,
                    post.ImagemBase64,
                    post.NumCurtidas,
                    post.NumComentarios,
                    post.TimeStamp
                );

            return Ok(resposta);
        }

        // Deletar Post por ID
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
        [HttpPost]
        [Route("Comentarios/{id}/{comentario}")]
        public IActionResult AddComentarioPost(Guid id, string comentario)
        {
            var postComentado = _pstRepository.Get(id);

            if (postComentado is null)
            {
                return NotFound("Post não Encontrado");
            }

            var usuario = _usrRepository.Get(ConfigUsuario.CurrentUserId);

            if (usuario is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            var novaMensagem = new Mensagem(usuario.Id, id, comentario);

            _msgRepository.Add(novaMensagem);

            postComentado.NumComentarios++;

            _pstRepository.Update(postComentado);

            var resposta = new ChatDto
                (
                    novaMensagem.Id,
                    novaMensagem.Remetente,
                    novaMensagem.Destinatario,
                    novaMensagem.Conteudo,
                    novaMensagem.TimeStamp
                );

            // ******* Area destinada aos ganhos de XP automatico da Rede ****** //
            Calculos c = new();

            c.GanharXP(200, ref usuario);

            _usrRepository.Update(usuario);
            // ******* Area destinada aos ganhos de XP automatico da Rede ****** //

            return Ok(resposta);
        }

        // Retornar Comentarios do Post
        [HttpGet]
        [Route("Comentarios/{id}")]
        public IActionResult GetComentariosPost(Guid id)
        {
            var comentarios = _msgRepository.RecebidasPor(id);
            var comentariosFormatados = new List<ComentariosDto>();

            if (comentarios is null || comentarios.Count == 0)
            {
                return NotFound("Não foi Encontrado Nenhum Comentario");
            }

            foreach (var comentario in comentarios)
            {
                Usuario? usuario = _usrRepository.Get(comentario.Remetente);

                if (usuario is null)
                {
                    return Problem("Algo Inesperado Ocorreu, Usuario que Comentou não Encontrado");
                }

                comentariosFormatados.Add(new ComentariosDto
                    (
                        comentario.Remetente,
                        comentario.Destinatario,
                        usuario.ImagemPerfil,
                        usuario.Nome,
                        comentario.Conteudo,
                        comentario.timeStamp
                    ));
            }

            return Ok(comentariosFormatados);
        }

        // Posts/Feed:
        [HttpGet]
        [Route("Feed")]
        public IActionResult GetFeedUsr()
        {
            var usuario = _usrRepository.Get(ConfigUsuario.CurrentUserId);

            if (usuario is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            var posts = _pstRepository.GetFeedUsr(usuario.Id);

            if (posts is null || posts.Count == 0)
            {
                return NotFound("Nenhum Post Criado por Amigos Encontrado");
            }

            return Ok(posts);
        }

        // Posts/Interagir:
        // Adicionar uma curtida a um post
        [HttpPost]
        [Route("Interagir/Curtir/{idPost}")]
        public IActionResult AddCurtidaPost(Guid idPost)
        {
            var postCurtido = _pstRepository.Get(idPost);

            if (postCurtido is null)
            {
                return NotFound();
            }

            var usuario = _usrRepository.Get(ConfigUsuario.CurrentUserId);

            if (usuario is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            Curtida curtida = new Curtida(usuario.Id, idPost);

            var jaCurtiu = _crtRepository.JaCurtiu(curtida);

            if (jaCurtiu != null)
            {
                return NotFound("O Post Já foi Curtido Antes");

            }

            _crtRepository.Add(curtida, ref postCurtido);
            _pstRepository.Update(postCurtido);

            // ******* Area destinada aos ganhos de XP automatico da Rede ****** //
            Calculos c = new();

            c.GanharXP(50, ref usuario);

            _usrRepository.Update(usuario);
            // ******* Area destinada aos ganhos de XP automatico da Rede ****** //

            var resposta = new PostPadraoDto
                (
                    postCurtido.Id,
                    postCurtido.IdUsuario,
                    postCurtido.Titulo,
                    postCurtido.Descricao,
                    postCurtido.NumCurtidas,
                    postCurtido.NumComentarios,
                    postCurtido.TimeStamp
                );

            return Ok(resposta);
        }

        // Adicionar uma curtida a um post
        [HttpPost]
        [Route("Interagir/Descurtir/{idPost}")]
        public IActionResult RemoverCurtidaPost(Guid idPost)
        {
            var postDescurtido = _pstRepository.Get(idPost);

            if (postDescurtido is null)
            {
                return NotFound();
            }

            var usuario = _usrRepository.Get(ConfigUsuario.CurrentUserId);

            if (usuario is null)
            {
                return NotFound("Usuario não Encontrado");
            }

            Curtida descurtida = new Curtida(usuario.Id, idPost);

            var curtiuAntes = _crtRepository.JaCurtiu(descurtida);

            if (curtiuAntes is null)
            {
                return NotFound("Post Nunca foi Curtido Por Esse Usuario Antes");
            }

            _crtRepository.Delete(curtiuAntes, ref postDescurtido);
            _pstRepository.Update(postDescurtido);

            var resposta = new PostPadraoDto
                (
                    postDescurtido.Id,
                    postDescurtido.IdUsuario,
                    postDescurtido.Titulo,
                    postDescurtido.Descricao,
                    postDescurtido.NumCurtidas,
                    postDescurtido.NumComentarios,
                    postDescurtido.TimeStamp
                );

            return Ok(resposta);
        }

        // Posts/Pesquisa
        // Retornar somente posts que correspondem a determinada pesquisa
        [HttpGet]
        [Route("Pesquisa/{termo}")]
        public IActionResult GetPostsDoUsuario(string termo)
        {
            var postsCorrespondentes = _pstRepository.GetPostsPesquisa(termo);

            if (postsCorrespondentes is null || postsCorrespondentes.Count == 0)
            {
                return NotFound("Nenhum Post Correspondente Encontrado");
            }

            return Ok(postsCorrespondentes);
        }

        // Posts/DoUsuario:
        // Retornar somente posts feitos por aquele usuario
        [HttpGet]
        [Route("DoUsuario")]
        public IActionResult GetPostsDoUsuario()
        {
            var usuario = _usrRepository.Get(ConfigUsuario.CurrentUserId);

            if (usuario is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            var posts = _pstRepository.PostadosPor(usuario.Id);

            if (posts is null || posts.Count == 0)
            {
                return NotFound("Nenhum Post Feito Ainda...");
            }

            return Ok(posts);
        }
    }
}
