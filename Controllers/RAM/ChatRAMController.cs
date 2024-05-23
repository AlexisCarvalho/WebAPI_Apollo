using Microsoft.AspNetCore.Mvc;
using WebAPI_Apollo.Infraestrutura;
using WebAPI_Apollo.Infraestrutura.Services.Repository.RAM;
using WebAPI_Apollo.Model.DTOs;
using WebAPI_Apollo.Model.Interacoes;

namespace WebAPI_Apollo.Controllers.RAM
{
    [ApiController]
    [Route("RAM/Chat")]
    public class ChatRAMController : ControllerBase
    {
        private readonly MensagemRepositoryRAM _msgRepository = new();
        private readonly InformHomeRepositoryRAM _infHomeRepository = new();
        private readonly UsuarioRepositoryRAM _usrRepository = new();

        // O Authorize daqui faz com que ele precise da verificação do token pra rodar
        // Ta desativado pra tu testar sem ter que logar toda vez, ele bloqueia todas as rotas
        // Até alguém cadastrado fazer login
        // (Deixei um usuario padrão lá causo queira testar a rota Auth)
        //  Email: Alexis@gmail.com Senha: 123456)

        // Falta adicionar a parte de mandar apenas para amigos
        //[Authorize]
        [HttpPost]
        [Route("{remetente}/{destinatario}/{conteudo}")]
        public IActionResult AddMsg(Guid remetente, Guid destinatario, string conteudo) 
        { 
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

        // Deletar Tudo da Memória
        //[Authorize]
        [HttpDelete]
        [Route("Destroy")]
        public IActionResult DeleteAllDataRAM()
        {
            VolatileContext.Mensagens.Clear();

            return NoContent();
        }
    }
}
