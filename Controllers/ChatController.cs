using Microsoft.AspNetCore.Mvc;
using WebAPI_Apollo.Model.DTOs;
using WebAPI_Apollo.Model.Interacoes;
using WebAPI_Apollo.Model.ViewModel;

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
        [Route("{remetente}/{destinatario}/{conteudo}")]
        public IActionResult AddMsg(Guid remetente, Guid destinatario, string conteudo)
        {
            var amigos = _amzdRepository.VerificarAmizade(new Amizade(remetente, destinatario));

            if (amigos is null)
            {
                return BadRequest("Usuarios não são amigos");
            }

            var novaMensagem = new Mensagem(remetente, destinatario, conteudo);

            var usuario = _usrRepository.Get(remetente);
            var amigo = _usrRepository.Get(destinatario);
            var home = _infHomeRepository.GetViaUsr(destinatario);


            if (usuario is null)
            {
                return NotFound("Usuario não encontrado");
            }

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

            var resposta = new ChatDto
                (
                    novaMensagem.Id,
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

            var resposta = new ChatDto
                (
                    mensagem.Id,
                    mensagem.Remetente,
                    mensagem.Destinatario,
                    mensagem.Conteudo,
                    mensagem.TimeStamp
                );

            return Ok(resposta);
        }

        // Mensagens trocadas entre dois ids
        [HttpGet]
        [Route("{remetente}/{destinatario}")]
        public IActionResult GetEnviadasEntreIDs(Guid remetente, Guid destinatario)
        {
            var mensagens = _msgRepository.EnviadasEntre(remetente, destinatario);

            if (mensagens is null || mensagens.Count == 0)
            {
                return NotFound();
            }

            return Ok(mensagens);
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

            var resposta = new ChatDto
                (
                    mensagem.Id,
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
        [Route("Usuario/Enviadas/{id}")]
        public IActionResult GetEnviadas(Guid id)
        {
            var mensagens = _msgRepository.EnviadasPor(id);

            if (mensagens is null || mensagens.Count == 0)
            {
                return NotFound();
            }

            return Ok(mensagens);
        }

        [HttpGet]
        [Route("Usuario/Recebidas/{id}")]
        public IActionResult GetRecebidas(Guid id)
        {
            var mensagens = _msgRepository.RecebidasPor(id);

            if (mensagens is null || mensagens.Count == 0)
            {
                return NotFound();
            }

            return Ok(mensagens);
        }
    }
}
