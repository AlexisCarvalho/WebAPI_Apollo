using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebAPI_Apollo.Infraestrutura;
using WebAPI_Apollo.Infraestrutura.Services;
using WebAPI_Apollo.Infraestrutura.Services.Repository.RAM;
using WebAPI_Apollo.Model;
using WebAPI_Apollo.Model.DTOs;
using WebAPI_Apollo.Model.Interacoes;

namespace WebAPI_Apollo.Controllers.DB
{
    [ApiController]
    [Route("RAM/Usuario")]
    public class UsuarioRAMController : ControllerBase
    {
        private readonly UsuarioRepositoryRAM _usrRepository = new();
        private readonly EstatisticasRepositoryRAM _estRepository = new();
        private readonly NotificacaoRepositoryRAM _ntfRepository = new();
        private readonly InformHomeRepositoryRAM _infHomeRepository = new();
        private readonly AmizadeRepositoryRAM _amzdRepository = new();

        /*

            * ------------------------------------ *
            *               PAREI AQUI             *
            * ------------------------------------ *

            E procurar o resto do que falta pq não lembro
            Se vira ai men... acabei de lembrar
            Tem que adicionar nas notificacoes como não lida
            Vulgo aumentar o numero de notificacoes
            Pensar na lógica pra isso depois

            Coisa nova, anterior adiantado, preciso atualizar a 
            Home nas rotas que envolvem ela, como essa de adicionar 
            usuario, criar rota pra aceitar a solicitação e adicionar
            a futura lista de amigos (Já existe repositorio pra isso)
            falta implementar.

        */

        // Adicionar um Usuario
        //[Authorize]
        [HttpPost]
        [Route("{nome}/{email}/{senha}/{userName}/{palavraRecuperacao}/{dataNascimento}")]
        public IActionResult AddUsr(string nome, string email, string senha, string userName, string palavraRecuperacao, string dataNascimento)
        {
            DateTime dataConvertida;

            try
            {
                dataConvertida = RegexVerifier.dataNoPadrão(dataNascimento);
            }
            catch (FormatException)
            {
                return BadRequest("Data no Padrão Errado");
            }

            var novoUsuario = new Usuario(nome, email, senha, userName, palavraRecuperacao, dataConvertida);

            var existente = _usrRepository.verificarSeExisteEmailUsername(novoUsuario);

            if (existente)
            {
                return BadRequest("Usuario com mesmo username ou email existente");
            }

            InformHome informHome = new InformHome(novoUsuario.Id);

            _usrRepository.Add(novoUsuario);
            _infHomeRepository.Add(informHome);

            var resposta = new UsuarioDto(novoUsuario.Id, novoUsuario.Idade, novoUsuario.XP, novoUsuario.Level, novoUsuario.XP_ProximoNivel, novoUsuario.Nome, novoUsuario.Email, novoUsuario.Senha, novoUsuario.Esporte, novoUsuario.Genero, novoUsuario.UserName, novoUsuario.PalavraRecuperacao, novoUsuario.DataNascimento, novoUsuario.Peso, novoUsuario.Altura);
            return Ok(resposta);
        }

        // Adicionar XP em Usuarios especificos por um ID 
        //[Authorize]
        [HttpPost]
        [Route("{id}/{XP}")]
        public IActionResult AdcXPUsr(Guid id, int XP)
        {
            Calculos c = new Calculos();

            //var usuario = await context.Usuarios.FindAsync(id);
            var usuario = _usrRepository.Get(id);

            if (usuario is null)
            {
                return NotFound();
            }

            c.GanharXP(XP, ref usuario);

            _usrRepository.Update(usuario);

            var resposta = new UsuarioDto(usuario.Id, usuario.Idade, usuario.XP, usuario.Level, usuario.XP_ProximoNivel, usuario.Nome, usuario.Email, usuario.Senha, usuario.Esporte, usuario.Genero, usuario.UserName, usuario.PalavraRecuperacao, usuario.DataNascimento, usuario.Peso, usuario.Altura);
            return Ok(resposta);
        }

        // Retornar todos os Usuarios 
        //[Authorize]
        [HttpGet]
        public IActionResult GetAllUsr()
        {
            var usuarios = _usrRepository.GetAll();

            if (usuarios is null || usuarios.Count == 0)
            {
                return NotFound();
            }

            return Ok(usuarios);
        }

        // Obter informações por id 
        //[Authorize]
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetUsr(Guid id)
        {
            var usuario = _usrRepository.Get(id);

            if (usuario is null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // Atualizar informações por id 
        //[Authorize]
        [HttpPut]
        [Route("{id}/{nome}/{esporte}/{genero}/{userName}/{palavraRecuperacao}/{dataNascimento}/{peso}/{altura}")]
        public IActionResult AtualizarUsr(Guid id, string nome, string esporte, string genero, string userName, string palavraRecuperacao, string dataNascimento, float peso, float altura)
        {
            var calculos = new Calculos();

            var usuario = _usrRepository.Get(id);

            var usuarioSemelhante = _usrRepository.GetSemelhanteUserName(userName);

            if (usuarioSemelhante is null)
            {
                usuarioSemelhante = usuario;
            }

            if (usuario is null || usuarioSemelhante is null)
            {
                return NotFound();
            }

            if (usuarioSemelhante.Id != usuario.Id)
            {
                return BadRequest("Usuario com mesmo Username existente");
            }

            DateTime dataConvertida;
            try
            {
                dataConvertida = RegexVerifier.dataNoPadrão(dataNascimento);
            }
            catch (FormatException)
            {
                return BadRequest("Data no Padrão Errado"); 
            }

            // [Futura] Conversão e testes do padrão do Email

            if (usuario.Altura != altura || usuario.Peso != peso)
            {
                var estatisticas = _estRepository.Get(usuario.IdEstatisticas);

                if (estatisticas is null)
                {
                    if (peso != 0 && altura != 0)
                    {
                        usuario.Peso = peso;
                        usuario.Altura = altura;

                        Estatisticas est = new Estatisticas(calculos.CalcularIMC(usuario), calculos.CalcularAguaDiaria(true, usuario));

                        var ultimaEstatistica = _estRepository.GetLast();

                        // Código pra substituir o autoIncrement do Banco
                        if (ultimaEstatistica != null)
                        {
                            est.Id = ultimaEstatistica.Id + 1;
                        }
                        else
                        {
                            est.Id = 1;
                        }
                        // Visa manter o uso de id int ao invés de trocar pra Guid

                        _estRepository.Add(est);

                        ultimaEstatistica = _estRepository.GetLast();

                        if (ultimaEstatistica != null)
                        {
                            usuario.IdEstatisticas = ultimaEstatistica.Id;
                            _usrRepository.Update(usuario);
                        }
                    }
                    else if (peso == 0)
                    {
                        return BadRequest("Valor do Peso não pode ser 0");
                    }
                    else if (altura == 0)
                    {
                        return BadRequest("Valor da Altura não pode ser 0");
                    }
                }
                // Se não existe estatisticas cria, a não ser que peso e altura sejam 0, ai não faz nada
                else
                {
                    if (peso != 0 && altura != 0)
                    {
                        usuario.Peso = peso;
                        usuario.Altura = altura;
                        Estatisticas novasEstati = new Estatisticas(calculos.CalcularIMC(usuario), calculos.CalcularAguaDiaria(true, usuario));
                        novasEstati.Id = estatisticas.Id;

                        EstatisticasRepositoryRAM doisEstRepository = new();
                        doisEstRepository.Update(novasEstati); // ERRO ESTAVA OCORRENDO AO EXECUTAR ESTE UPDATE, criar outro repository arrumou
                    }
                    else if (peso == 0)
                    {
                        return BadRequest("Valor do Peso não pode ser 0");
                    }
                    else if (altura == 0)
                    {
                        return BadRequest("Valor da Altura não pode ser 0");
                    }
                }
                // Se existem atualiza
            }
            // Se o usuario mexer em altura ou peso, seu perfil é gerado novamente


            usuario.Nome = nome;
            usuario.Esporte = esporte;
            usuario.Genero = genero;
            usuario.UserName = userName;
            usuario.PalavraRecuperacao = palavraRecuperacao;
            usuario.DataNascimento = dataConvertida;
            usuario.Idade = calculos.CalcularIdade(dataConvertida);

            _usrRepository.Update(usuario);

            return Ok(new UsuarioDto(usuario.Id, usuario.Idade, usuario.XP, usuario.Level, usuario.XP_ProximoNivel, usuario.Nome, usuario.Email, usuario.Senha, usuario.Esporte, usuario.Genero, usuario.UserName, usuario.PalavraRecuperacao, usuario.DataNascimento, usuario.Peso, usuario.Altura));
        }

        // Deletar Usuario por ID
        //[Authorize]
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteUsr(Guid id)
        {
            var usuario = _usrRepository.Get(id);

            if (usuario is null)
            {
                return NotFound();
            }

            _usrRepository.Delete(usuario);

            return NoContent();
        }

        // Carregar a Home 
        //[Authorize]
        [HttpGet]
        [Route("Amizades/{idUsuario}")]
        public IActionResult GetAmigosUsr(Guid idUsuario)
        {
            var amizades = _amzdRepository.GetAllUsr(idUsuario);
            List<AmizadeListaAmigos> listaAmigos = new List<AmizadeListaAmigos>();

            foreach (var amizade in amizades)
            {
                if (amizade.Remetente == idUsuario)
                {
                    var amigo = _usrRepository.Get(amizade.Destinatario);

                    if (amigo != null)
                    {
                        listaAmigos.Add(new AmizadeListaAmigos(amigo.Id, amigo.Nome, "")); // Aqui que entrara a foto
                    }
                    else
                    {
                        Problem("Algum usuario da lista de amigos parece não existir de forma inesperada");
                    }
                }
                else
                {
                    var amigo = _usrRepository.Get(amizade.Remetente);

                    if (amigo != null)
                    {
                        listaAmigos.Add(new AmizadeListaAmigos(amigo.Id, amigo.Nome, "")); // Aqui que entrara a foto
                    }
                    else
                    {
                        Problem("Algum usuario da lista de amigos parece não existir de forma inesperada");
                    }
                }
            }

            if (listaAmigos is null || listaAmigos.Count == 0)
            {
                return NotFound("Nenhum Amigo Encontrado");
            }

            return Ok(listaAmigos);
        }

        // Adicionar Solicitacoes de Amizade 
        // Entre o conjunto de Solicitar Aceitar e Recusar atualmente existe
        // a chance de ambos mandarem um pro outro uma solicitaão antes de algum aceitar
        //[Authorize]
        [HttpPost]
        [Route("Amizades/Solicitar/{idUsuario}/{idAmigo}")]
        public IActionResult AdcSoliAmiza(Guid idUsuario, Guid idAmigo)
        {
            // Usuarios 
            var usuario = _usrRepository.Get(idUsuario);
            var quemEleQuerPedir = _usrRepository.Get(idAmigo);

            // Home do amigo atualizada com a notificação
            var homeAmigo = _infHomeRepository.GetViaUsr(idAmigo);

            if (usuario is null)
            {
                return NotFound("Usuario não encontrado");
            }

            if (quemEleQuerPedir is null)
            {
                return NotFound("Usuario solicitado não encontrado");
            }

            if(homeAmigo is null)
            {
                return Problem("Informações da Home do amigo não encontradas");
            }

            Amizade amizade = new(usuario.Id, quemEleQuerPedir.Id);

            var jaEAmigo = _amzdRepository.JaEAmigo(amizade);

            if (jaEAmigo != null)
            {
                return BadRequest("O Usuario requisitado já esta na sua lista de amigos");
            }

            string mensagem = $"Olá {quemEleQuerPedir.Nome.Split(" ")[0]}, gostaria de ser meu amigo aqui na {ConfSistema.NomeRedeSocial}. Seria ótimo trocar ideias e experiências.";

            Notificacao solicitacaoAmizade = new Notificacao(idUsuario, idAmigo, 1 , mensagem);
            // Tipo 1, solicitação de amizade

            var jaExisteSolicitacao = _ntfRepository.JaFoiNotificado(solicitacaoAmizade);

            if (jaExisteSolicitacao != null)
            {
                return BadRequest("Solicitação de amizade já enviada");
            }

            _ntfRepository.Add(solicitacaoAmizade);
            
            // Incrementa as solicitações de amizade do amigo
            homeAmigo.NumSolicitacoesAmizade++;
            _infHomeRepository.Update(homeAmigo);

            return Ok(solicitacaoAmizade);
        }

        // Aceitar solicitação de amizade
        //[Authorize]
        [HttpPost]
        [Route("Amizades/Aceitar/{idUsuario}/{idDeQuemPediu}")]
        public IActionResult AceitarSoliAmiza(Guid idUsuario, Guid idDeQuemPediu)
        {
            var pedidoExistente = _ntfRepository.JaFoiNotificado(new Notificacao(idDeQuemPediu, idUsuario, 1, ""));
            
            if (pedidoExistente is null)
            {
                return NotFound("Não Existe nenhum Pedido de Amizade pra Aceitar");
            }

            // Usuarios 
            var usuario = _usrRepository.Get(idUsuario);
            var quemPediu = _usrRepository.Get(idDeQuemPediu);

            // Home do amigo atualizada com a notificação
            var home = _infHomeRepository.GetViaUsr(idUsuario);
            var homeAmigo = _infHomeRepository.GetViaUsr(idDeQuemPediu);

            if (usuario is null)
            {
                return NotFound("Usuario não encontrado");
            }

            if (quemPediu is null)
            {
                return NotFound("Usuario que solicitou não encontrado");
            }

            if (home is null)
            {
                return Problem("Informações da Home não encontradas");
            }

            if (homeAmigo is null)
            {
                return Problem("Informações da Home do Amigo não encontradas");
            }

            string mensagem = $"{quemPediu.Nome.Split(" ")[0]}, agora somos amigos aqui na {ConfSistema.NomeRedeSocial}.";

            Notificacao resposta = new Notificacao(idUsuario, idDeQuemPediu, 3, mensagem);
            // Tipo 3, Resposta

            var jaExiste = _ntfRepository.JaFoiNotificado(resposta);

            if (jaExiste != null)
            {
                return BadRequest("Solicitação já aceita");
            }

            _ntfRepository.Add(resposta);

            home.NumSolicitacoesAmizade--;
            home.NumAmigos++;
            _infHomeRepository.Update(home);

            homeAmigo.NumNotificacoesNaoLidas++;
            _infHomeRepository.Update(homeAmigo);

            Amizade amizade = new(quemPediu.Id, usuario.Id);

            var jaEAmigo = _amzdRepository.JaEAmigo(amizade);

            if(jaEAmigo != null)
            {
                return BadRequest("O Usuario requisitado já esta na sua lista de amigos");
            }

            _amzdRepository.Add(amizade);

            return Ok(resposta);
        }

        // Aceitar solicitação de amizade
        //[Authorize]
        [HttpPost]
        [Route("Amizades/Recusar/{idUsuario}/{idDeQuemPediu}")]
        public IActionResult RecusarSoliAmiza(Guid idUsuario, Guid idDeQuemPediu)
        {
            var pedidoExistente = _ntfRepository.JaFoiNotificado(new Notificacao(idDeQuemPediu, idUsuario, 1, ""));

            if (pedidoExistente is null)
            {
                return NotFound("Não Existe nenhum Pedido de Amizade pra Recusar");
            }

            // Usuarios 
            var usuario = _usrRepository.Get(idUsuario);
            var quemPediu = _usrRepository.Get(idDeQuemPediu);

            // Home do amigo atualizada com a notificação
            var home = _infHomeRepository.GetViaUsr(idUsuario);
            var homeAmigo = _infHomeRepository.GetViaUsr(idDeQuemPediu);

            if (usuario is null)
            {
                return NotFound("Usuario não encontrado");
            }

            if (quemPediu is null)
            {
                return NotFound("Usuario que solicitou não encontrado");
            }

            if (home is null)
            {
                return Problem("Informações da Home não encontradas");
            }

            if (homeAmigo is null)
            {
                return Problem("Informações da Home do Amigo não encontradas");
            }

            string mensagem = $"{quemPediu.Nome.Split(" ")[0]}, infelizmente não posso aceitar seu pedido de amizade aqui na {ConfSistema.NomeRedeSocial}.";

            Notificacao resposta = new Notificacao(idUsuario, idDeQuemPediu, 3, mensagem);
            // Tipo 3, Resposta

            var jaExiste = _ntfRepository.JaFoiNotificado(resposta);

            if (jaExiste != null)
            {
                return BadRequest("Solicitação já recusada");
            }

            _ntfRepository.Add(resposta);

            home.NumSolicitacoesAmizade--;
            _infHomeRepository.Update(home);

            homeAmigo.NumNotificacoesNaoLidas++;
            _infHomeRepository.Update(homeAmigo);

            Amizade amizade = new(quemPediu.Id, usuario.Id);

            var jaEAmigo = _amzdRepository.JaEAmigo(amizade);

            if (jaEAmigo != null)
            {
                return BadRequest("O Usuario requisitado já esta na sua lista de amigos");
            }

            return Ok(resposta);
        }

        // Carregar a Home 
        //[Authorize]
        [HttpGet]
        [Route("Home/{idUsuario}")]
        public IActionResult GetHomeUsr(Guid idUsuario)
        {
            var homeUsuario = _infHomeRepository.GetViaUsr(idUsuario);

            if (homeUsuario is null)
            {
                return NotFound();
            }

            return Ok(new InformHomeDto(homeUsuario.IdUsuario, homeUsuario.NumNotificacoesNaoLidas, homeUsuario.NumSolicitacoesAmizade,homeUsuario.NumAmigos, homeUsuario.NumMensagensNaoLidas));
        }

        // Carregar as Notificacoes 
        //[Authorize]
        [HttpGet]
        [Route("Notificacoes/{idUsuario}")]
        public IActionResult GetNotiUsr(Guid idUsuario)
        {
            var notiUsuario = _ntfRepository.GetAllUsr(idUsuario);

            if (notiUsuario is null || notiUsuario.Count == 0)
            {
                return NotFound();
            }

            return Ok(notiUsuario);
        }

        // Rota para carregar o perfil do usuario *
        //[Authorize]
        [HttpGet]
        [Route("Perfil/{id}")]
        public IActionResult PerfilUsr(Guid id)
        {
            var usuario = _usrRepository.Get(id);

            Calculos c = new Calculos();

            if (usuario is null)
            {
                return NotFound();
            }

            var estatisticas = _estRepository.Get(usuario.IdEstatisticas);

            if (estatisticas is null)
            {
                if (usuario.Peso != 0 && usuario.Altura != 0)
                {
                    Estatisticas est = new Estatisticas(c.CalcularIMC(usuario), c.CalcularAguaDiaria(true, usuario));
                    var ultimaEstatistica = _estRepository.GetLast();

                    // Código pra substituir o autoIncrement do Banco
                    if (ultimaEstatistica != null)
                    {
                        est.Id = ultimaEstatistica.Id + 1;
                    }
                    else
                    {
                        est.Id = 1;
                    }
                    // Visa manter o uso de id int ao invés de trocar pra Guid

                    _estRepository.Add(est);

                    ultimaEstatistica = _estRepository.GetLast();
                    if (ultimaEstatistica != null)
                    {
                        usuario.IdEstatisticas = ultimaEstatistica.Id;
                        _usrRepository.Update(usuario);
                    }
                    var resposta = new PerfilUsuarioDto(usuario.Nome, usuario.Altura, usuario.Peso, est.IMC, est.AguaDiaria, usuario.Level, usuario.XP);
                    return Ok(resposta);
                }
                else
                {
                    return BadRequest("Não é possivel gerar o Perfil sem Peso e Altura");
                }
            }
            else
            {
                var resposta = new PerfilUsuarioDto(usuario.Nome, usuario.Altura, usuario.Peso, estatisticas.IMC, estatisticas.AguaDiaria, usuario.Level, usuario.XP);
                return Ok(resposta);
            }
        }

        // Efetuar Login 
        //[Authorize]
        [HttpGet]
        [Route("Login/{email}/{senha}")]
        public IActionResult Login(string email, string senha)
        {
            var usuario = _usrRepository.GetViaLogin(email, senha);

            if (usuario is null)
            {
                return NotFound("Email ou Senha Errados");
            }

            return Ok(usuario);
        }

        // Rota para alterar as informações do Login
        //[Authorize]
        [HttpPut]
        [Route("Login/{id}/{email}/{senha}")]
        public IActionResult LoginUpdate(Guid id, string email, string senha)
        {
            var usuario = _usrRepository.Get(id);

            if(usuario is null)
            {
                return NotFound("Usuario Não Encontrado");
            }

            // Lógica pra impedir a troca pra email existente
            var usuarioSemelhante = _usrRepository.GetSemelhanteEmail(email);

            if (usuarioSemelhante is null)
            {
                usuarioSemelhante = usuario;
            }

            if (usuario is null || usuarioSemelhante is null)
            {
                return NotFound();
            }

            if (usuarioSemelhante.Id != usuario.Id)
            {
                return BadRequest("Usuario com mesmo Email existente");
            }

            usuario.Email = email;
            usuario.Senha = senha;

            _usrRepository.Update(usuario);

            return Ok(usuario);
        }

        // Recuperar senha simples
        //[Authorize]
        [HttpGet]
        [Route("Recuperacao/{email}/{palavraRecuperacao}")]
        public IActionResult RecuperarSenha(string email, string palavraRecuperacao)
        {
            var usuario = _usrRepository.RecuperarSenha(email, palavraRecuperacao);

            if (usuario is null)
            {
                return NotFound();
            }

            string caracteresPermitidos = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+-=[]{}|;:,.<>?";

            StringBuilder sb = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < 10; i++)
            {
                int index = random.Next(caracteresPermitidos.Length);
                sb.Append(caracteresPermitidos[index]);
            }

            string senhaAleatoria = sb.ToString();

            string resposta = $"{usuario.Nome}, agradecemos por usar a rede {ConfSistema.NomeRedeSocial}, sua nova senha temporaria pra acesso é: {senhaAleatoria}";

            usuario.Senha = senhaAleatoria;
            _usrRepository.Update(usuario);

            return Ok(resposta);
        }

        // Deletar Tudo da Memória
        //[Authorize]
        [HttpDelete]
        [Route("Destroy")]
        public IActionResult DeleteAllDataRAM()
        {
            VolatileContext.Estatisticas.Clear();
            VolatileContext.Usuarios.Clear();
            VolatileContext.Notificacoes.Clear();
            VolatileContext.InformHome.Clear();
            VolatileContext.Amizades.Clear();

            return NoContent();
        }

        // Deletar Tudo da Memória
        //[Authorize]
        [HttpDelete]
        [Route("Destroy/Estatisticas")]
        public IActionResult DeleteAllDataRAMEstatisticas()
        {
            VolatileContext.Estatisticas.Clear();

            return NoContent();
        }
    }
}
