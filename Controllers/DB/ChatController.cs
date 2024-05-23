using Microsoft.AspNetCore.Mvc;
using WebAPI_Apollo.Model.DTOs;
using WebAPI_Apollo.Model.Interacoes;
using WebAPI_Apollo.Model.ViewModel;

namespace WebAPI_Apollo.Controllers.DB
{
    [ApiController]
    [Route("Chat")]
    public class ChatController : ControllerBase
    {
        private readonly IMensagemRepository _msgRepository;

        public ChatController(IMensagemRepository msgRepository)
        {
            _msgRepository = msgRepository ?? throw new ArgumentNullException();
        }

        // O Authorize daqui faz com que ele precise da verificação do token pra rodar
        // Ta desativado pra tu testar sem ter que logar toda vez, ele bloqueia todas as rotas
        // Até alguém cadastrado fazer login
        // (Deixei um usuario padrão lá causo queira testar a rota Auth)
        //  Email: Alexis@gmail.com Senha: 123456)

        //[Authorize]
        [Obsolete("Alterar conforme o RAM")]
        [HttpPost]
        [Route("{remetente}/{destinatario}/{conteudo}")]
        public IActionResult AddMsg(Guid remetente, Guid destinatario, string conteudo)
        {
            var novaMensagem = new Mensagem(remetente, destinatario, conteudo);

            _msgRepository.Add(novaMensagem);

            var resposta = new ChatDto(novaMensagem.Id, novaMensagem.Remetente, novaMensagem.Destinatario, novaMensagem.Conteudo, novaMensagem.TimeStamp);
            return Ok(resposta);
        }

        // Retornar todas as Mensagens
        //[Authorize]
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
        //[Authorize]
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetMsg(int id)
        {
            var mensagem = _msgRepository.Get(id);

            if (mensagem is null)
            {
                return NotFound();
            }

            return Ok(new ChatDto(mensagem.Id, mensagem.Remetente, mensagem.Destinatario, mensagem.Conteudo, mensagem.TimeStamp));
        }

        // Mensagens trocadas entre dois ids
        //[Authorize]
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
        //[Authorize]
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

            return Ok(new ChatDto(mensagem.Id, mensagem.Remetente, mensagem.Destinatario, mensagem.Conteudo, mensagem.TimeStamp));
        }

        // Deletar Mensagem por ID
        //[Authorize]
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

        // Obter informações por ID
        // Mensagens Enviadas para aquele ID e Recebidas por aquele ID
        //[Authorize]
        [HttpGet]
        [Route("Enviadas/{id}")]
        public IActionResult GetEnviadas(Guid id)
        {
            var mensagens = _msgRepository.EnviadasPor(id);

            if (mensagens is null || mensagens.Count == 0)
            {
                return NotFound();
            }

            return Ok(mensagens);
        }

        //[Authorize]
        [HttpGet]
        [Route("Recebidas/{id}")]
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
