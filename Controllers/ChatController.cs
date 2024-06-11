using Microsoft.AspNetCore.Mvc;
using WebAPI_Apollo.Domain.DTOs;
using WebAPI_Apollo.Domain.Model.Interacoes;
using WebAPI_Apollo.Domain.Model.Interfaces;
using WebAPI_Apollo.Infraestrutura;

namespace WebAPI_Apollo.Controllers
{
    [ApiController]
    [Route("Chat")]
    public class ChatController : ControllerBase
    {
        private readonly IMensagemRepository _msgRepository;
        private readonly IInformHomeRepository _infHomeRepository;
        private readonly IUsuarioRepository _usrRepository;
        private readonly IAmizadeRepository _amzdRepository;

        public ChatController(IMensagemRepository msgRepository, IInformHomeRepository infHomeRepository, IUsuarioRepository usrRepository, IAmizadeRepository amzdRepository)
        {
            _msgRepository = msgRepository ?? throw new ArgumentNullException();
            _infHomeRepository = infHomeRepository ?? throw new ArgumentNullException();
            _usrRepository = usrRepository ?? throw new ArgumentNullException();
            _amzdRepository = amzdRepository ?? throw new ArgumentNullException();
        }

        // Chat:

        [HttpPost]
        [Route("{destinatario}/{conteudo}")]
        public IActionResult AddMsg(Guid destinatario, string conteudo)
        {
            var usuario = ConfigUsuario.CurrentUser;

            if (usuario is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            if (usuario.Id == destinatario)
            {
                return BadRequest("Não é possível mandar uma mensagem para si mesmo");
            }

            var amigos = _amzdRepository.VerificarAmizade(new Amizade(usuario.Id, destinatario));

            if (amigos is null)
            {
                return BadRequest("Usuarios Não São Amigos");
            }

            var novaMensagem = new Mensagem(usuario.Id, destinatario, conteudo);

            var amigo = _usrRepository.Get(destinatario);
            var home = _infHomeRepository.GetViaUsr(destinatario);

            if (amigo is null)
            {
                return NotFound("Usuario solicitado não encontrado");
            }

            if (home is null)
            {
                return Problem("Home do Usuario não Encontrada");
            }

            home.NumMensagensNaoLidas++;

            _infHomeRepository.Update(home);
            _msgRepository.Add(novaMensagem);

            var resposta = new ChatDtoRetornoPost
                (
                    novaMensagem.Remetente,
                    novaMensagem.Destinatario,
                    novaMensagem.Conteudo,
                    novaMensagem.TimeStamp
                );

            return Ok(resposta);
        }

        // Retornar todas as Mensagens
        [HttpGet]
        public IActionResult GetAllMsg()
        {
            var mensagem = _msgRepository.GetAll();

            if (mensagem is null || mensagem.Count == 0)
            {
                return NotFound();
            }

            return Ok(mensagem);
        }

        // Retornar mensagem
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetMsg(int id)
        {
            var mensagem = _msgRepository.Get(id);

            if (mensagem is null)
            {
                return NotFound();
            }

            var resposta = new ChatDtoRetornoPost
                (
                    mensagem.Remetente,
                    mensagem.Destinatario,
                    mensagem.Conteudo,
                    mensagem.TimeStamp
                );

            return Ok(resposta);
        }

        // Atualizar informações por id
        // Isso pra editar a mensagem já enviada (deixar pra mexer no final)
        [HttpPut]
        [Route("{id}/{conteudo}")]
        public IActionResult AtualizarMsg(int id, string conteudo)
        {
            var mensagem = _msgRepository.Get(id);

            if (mensagem is null)
            {
                return NotFound();
            }

            mensagem.Conteudo = conteudo;
            mensagem.TimeStamp = DateTime.Now;

            _msgRepository.Update(mensagem);

            var resposta = new ChatDtoRetornoPost
                (
                    mensagem.Remetente,
                    mensagem.Destinatario,
                    mensagem.Conteudo,
                    mensagem.TimeStamp
                );

            return Ok(resposta);
        }

        // Deletar Mensagem por ID
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var mensagem = _msgRepository.Get(id);

            if (mensagem is null)
            {
                return NotFound();
            }

            _msgRepository.Delete(mensagem);

            return NoContent();
        }

        // Chat/Usuario:

        // Obter informações por ID
        // Mensagens Enviadas para aquele ID e Recebidas por aquele ID
        [HttpGet]
        [Route("Usuario/Enviadas")]
        public IActionResult GetEnviadas()
        {
            var usuario = ConfigUsuario.CurrentUser;

            if (usuario is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            var mensagens = _msgRepository.EnviadasPor(usuario.Id);

            if (mensagens is null || mensagens.Count == 0)
            {
                return NotFound("Nenhuma Mensagem Enviada");
            }

            return Ok(mensagens);
        }

        [HttpGet]
        [Route("Usuario/Recebidas")]
        public IActionResult GetRecebidas()
        {
            var usuario = ConfigUsuario.CurrentUser;

            if (usuario is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            var mensagens = _msgRepository.RecebidasPor(usuario.Id);

            if (mensagens is null || mensagens.Count == 0)
            {
                return NotFound("Nenhuma Mensagem Recebida");
            }

            return Ok(mensagens);
        }

        // Mensagens trocadas entre dois ids
        [HttpGet]
        [Route("Usuario/TrocadasCom/{destinatario}")]
        public IActionResult GetEnviadasEntreIDs(Guid destinatario)
        {
            var usuario = ConfigUsuario.CurrentUser;

            if (usuario is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            var homeUsuario = _infHomeRepository.GetViaUsr(usuario.Id);

            if (homeUsuario is null)
            {
                return Problem("Home do Usuario Não Existe");
            }

            var mensagens = _msgRepository.EnviadasEntre(usuario.Id, destinatario);

            homeUsuario.NumMensagensNaoLidas = 0;

            _infHomeRepository.Update(homeUsuario);

            if (mensagens is null || mensagens.Count == 0)
            {
                return NotFound();
            }

            return Ok(mensagens);
        }
    }
}
