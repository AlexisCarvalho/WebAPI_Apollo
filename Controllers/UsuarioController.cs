using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebAPI_Apollo.Application.Services;
using WebAPI_Apollo.Domain.DTOs;
using WebAPI_Apollo.Domain.Model;
using WebAPI_Apollo.Domain.Model.Interacoes;
using WebAPI_Apollo.Domain.Model.Interfaces;
using WebAPI_Apollo.Infraestrutura;
using WebAPI_Apollo.Infraestrutura.Repository.RAM;

namespace WebAPI_Apollo.Controllers
{
    [ApiController]
    [Route("Usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly IInformHomeRepository _infHomeRepository;
        private readonly IUsuarioRepository _usrRepository;
        private readonly IAmizadeRepository _amzdRepository;
        private readonly IEstatisticasRepository _estRepository;
        private readonly INotificacaoRepository _ntfRepository;

        public UsuarioController(IInformHomeRepository infHomeRepository, IUsuarioRepository usrRepository, IAmizadeRepository amzdRepository, IEstatisticasRepository estRepository, INotificacaoRepository ntfRepository)
        {
            _infHomeRepository = infHomeRepository ?? throw new ArgumentNullException();
            _usrRepository = usrRepository ?? throw new ArgumentNullException();
            _amzdRepository = amzdRepository ?? throw new ArgumentNullException();
            _estRepository = estRepository ?? throw new ArgumentNullException();
            _ntfRepository = ntfRepository ?? throw new ArgumentNullException();
        }

        // Usuario:
        // Adicionar um Usuario
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

            var usuario = new Usuario(nome, email, senha, userName, palavraRecuperacao, dataConvertida);

            var existenteEmail = _usrRepository.GetSemelhanteEmail(usuario.Email).Result;
            var existenteUserName = _usrRepository.GetSemelhanteUserName(usuario.UserName).Result;

            if (existenteEmail != null)
            {
                return BadRequest("Usuario com mesmo email existente");
            }

            if (existenteUserName != null)
            {
                return BadRequest("Usuario com mesmo username existente");
            }

            InformHome informHome = new InformHome(usuario.Id);

            _usrRepository.Add(usuario);
            _infHomeRepository.Add(informHome);

            var resposta = new UsuarioDto
                (usuario.Id, usuario.Idade, usuario.XP,
                usuario.Level, usuario.XP_ProximoNivel,
                usuario.Nome, usuario.Email, usuario.Senha,
                usuario.Esporte, usuario.Genero, usuario.UserName,
                usuario.PalavraRecuperacao, usuario.DataNascimento,
                usuario.Peso, usuario.Altura, usuario.ImagemPerfil);

            return Ok(resposta);
        }

        // Adicionar XP em Usuarios especificos por um ID 
        [HttpPost]
        [Route("{XP}")]
        public IActionResult AdcXPUsr(int XP)
        {
            Calculos c = new();

            if (ConfigUsuario.CurrentUser is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            var usuario = ConfigUsuario.CurrentUser;

            c.GanharXP(XP, ref usuario);             // TODO: Arrumar isso aqui depois

            _usrRepository.Update(usuario);

            var resposta = new UsuarioDto
                (
                    usuario.Id, usuario.Idade, usuario.XP,
                    usuario.Level, usuario.XP_ProximoNivel,
                    usuario.Nome, usuario.Email, usuario.Senha,
                    usuario.Esporte, usuario.Genero, usuario.UserName,
                    usuario.PalavraRecuperacao, usuario.DataNascimento,
                    usuario.Peso, usuario.Altura, usuario.ImagemPerfil
                );

            return Ok(resposta);
        }

        // Obter informações do usuario logado
        [HttpGet]
        public IActionResult GetUsr()
        {
            if (ConfigUsuario.CurrentUser is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            var usuario = ConfigUsuario.CurrentUser;

            var resposta = new UsuarioDto
                (usuario.Id, usuario.Idade, usuario.XP,
                usuario.Level, usuario.XP_ProximoNivel,
                usuario.Nome, usuario.Email, usuario.Senha,
                usuario.Esporte, usuario.Genero, usuario.UserName,
                usuario.PalavraRecuperacao, usuario.DataNascimento,
                usuario.Peso, usuario.Altura, usuario.ImagemPerfil);

            return Ok(resposta);
        }

        // Obter informações do usuario
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetUsuarioSist(Guid id)
        {
            var usuario = _usrRepository.Get(id).Result;

            if (usuario is null)
            {
                return BadRequest("Nenhum Usuario Encontrado");
            }

            var resposta = new UsuarioDto(usuario.Id, usuario.Idade, usuario.XP, usuario.Level, usuario.XP_ProximoNivel, usuario.Nome, usuario.Email, usuario.Senha, usuario.Esporte, usuario.Genero, usuario.UserName, usuario.PalavraRecuperacao, usuario.DataNascimento, usuario.Peso, usuario.Altura, usuario.ImagemPerfil);
            return Ok(resposta);
        }

        // Atualizar informações do usuario logado
        [HttpPut]
        [Route("{nome}/{esporte}/{genero}/{userName}/{palavraRecuperacao}/{dataNascimento}/{peso}/{altura}")]
        public IActionResult AtualizarUsr(string nome, string esporte, string genero, string userName, string palavraRecuperacao, string dataNascimento, float peso, float altura)
        {
            var calculos = new Calculos();

            var usuario = ConfigUsuario.CurrentUser;

            var usuarioSemelhante = _usrRepository.GetSemelhanteUserName(userName).Result;

            if (usuarioSemelhante is null)
            {
                usuarioSemelhante = usuario;
            }

            if (usuario is null || usuarioSemelhante is null)
            {
                return BadRequest("Nenhum Usuario Logado");
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

            // TODO: Conversão e testes do padrão do Email

            if (usuario.Altura != altura || usuario.Peso != peso)
            {
                var estatisticas = _estRepository.Get(usuario.IdEstatisticas).Result;

                if (estatisticas is null)
                {
                    if (peso != 0 && altura != 0)
                    {
                        usuario.Peso = peso;
                        usuario.Altura = altura;

                        Estatisticas est = new(calculos.CalcularIMC(usuario), calculos.CalcularAguaDiaria(true, usuario));

                        var ultimaEstatistica = _estRepository.GetLast().Result;

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

                        ultimaEstatistica = _estRepository.GetLast().Result;

                        if (ultimaEstatistica != null)
                        {
                            usuario.IdEstatisticas = ultimaEstatistica.Id;
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

            var resposta = new UsuarioDto
                (usuario.Id, usuario.Idade, usuario.XP,
                usuario.Level, usuario.XP_ProximoNivel,
                usuario.Nome, usuario.Email, usuario.Senha,
                usuario.Esporte, usuario.Genero, usuario.UserName,
                usuario.PalavraRecuperacao, usuario.DataNascimento,
                usuario.Peso, usuario.Altura, usuario.ImagemPerfil);

            return Ok(resposta);
        }

        // Deletar Usuario logado
        [HttpDelete]
        public IActionResult DeleteUsr()
        {
            var usuario = ConfigUsuario.CurrentUser;

            if (usuario is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            var estUsuario = _estRepository.Get(usuario.IdEstatisticas).Result;

            if (estUsuario != null)
            {
                _estRepository.Delete(estUsuario);
            }

            var infHomeUsr = _infHomeRepository.GetViaUsr(usuario.Id).Result;

            if (infHomeUsr is null)
            {
                return Problem("Informações da Home não podem ser nulas");
            }

            // Limpa todas as informações e referências do usuario
            // no sistema incluindo suas amizades.
            _infHomeRepository.Delete(infHomeUsr);
            _amzdRepository.DeletarReferencias(usuario.Id);

            // Todas essa são solicitações de amizade enviadas pelo usuario
            var notificacoesDoUsr = _ntfRepository.GetAllEnviadasNotiAmizadeUsr(usuario.Id).Result;

            foreach (var noti in notificacoesDoUsr)
            {
                var homeAmigo = _infHomeRepository.GetViaUsr(noti.destinatario).Result;

                if (homeAmigo is null)
                {
                    return Problem();
                }

                homeAmigo.NumSolicitacoesAmizade--;

                _infHomeRepository.Update(homeAmigo);
            }

            _ntfRepository.DeletarReferencias(usuario.Id);
            _usrRepository.Delete(usuario);

            return NoContent();
        }

        // Usuario/Amizades:
        // Obter amizades do Usuario Logado
        [HttpGet]
        [Route("Amizades")]
        public IActionResult GetAmigosUsr()
        {
            var usuario = ConfigUsuario.CurrentUser;

            if (usuario is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            var amizades = _amzdRepository.GetAllUsr(usuario.Id).Result;
            var listaAmigos = new List<AmizadeListaAmigos>();

            foreach (var amizade in amizades)
            {
                if (amizade.Remetente == usuario.Id)
                {
                    var amigo = _usrRepository.Get(amizade.Destinatario).Result;

                    if (amigo != null)
                    {
                        listaAmigos.Add(new AmizadeListaAmigos(amigo.Id, amigo.Nome, amigo.ImagemPerfil)); // Aqui que entrara a foto
                    }
                    else
                    {
                        return Problem("Algum usuario da lista de amigos parece não existir de forma inesperada");
                    }
                }
                else
                {
                    var amigo = _usrRepository.Get(amizade.Remetente).Result;

                    if (amigo != null)
                    {
                        listaAmigos.Add(new AmizadeListaAmigos(amigo.Id, amigo.Nome, amigo.ImagemPerfil)); // Aqui que entrara a foto
                    }
                    else
                    {
                        return Problem("Algum usuario da lista de amigos parece não existir de forma inesperada");
                    }
                }
            }

            if (listaAmigos is null || listaAmigos.Count == 0)
            {
                return NotFound("Nenhum Amigo Encontrado");
            }

            return Ok(listaAmigos);
        }

        // Pesquisar por amigos
        [HttpGet]
        [Route("Amizades/Procurar/{nome}")]
        public IActionResult PesquisarAmigo(string nome)
        {
            var amigosCorrespontes = _usrRepository.GetUsuariosNome(nome).Result;

            if (amigosCorrespontes is null || amigosCorrespontes.Count == 0)
            {
                return NotFound("Nenhum Usuario Correspondente Encontrado");
            }

            return Ok(amigosCorrespontes);
        }

        // Adicionar Solicitacoes de Amizade 
        // Entre o conjunto de Solicitar Aceitar e Recusar atualmente existe
        // a chance de ambos mandarem um pro outro uma solicitaão antes de algum aceitar
        [HttpPost]
        [Route("Amizades/Solicitar/{idAmigo}")]
        public IActionResult AdcSoliAmiza(Guid idAmigo)
        {
            // Usuarios 
            var usuario = ConfigUsuario.CurrentUser;
            var solicitado = _usrRepository.Get(idAmigo).Result;

            // Home do amigo atualizada com a notificação
            var homeAmigo = _infHomeRepository.GetViaUsr(idAmigo).Result;

            if (usuario is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            if (solicitado is null)
            {
                return NotFound("Usuario solicitado não encontrado");
            }

            if (homeAmigo is null)
            {
                return Problem("Informações da Home do amigo não encontradas");
            }

            if (usuario.Id == solicitado.Id)
            {
                return BadRequest("Você não pode enviar uma solicitação de amizade para si mesmo");
            }

            Amizade amizade = new(usuario.Id, solicitado.Id);

            var jaEAmigo = _amzdRepository.VerificarAmizade(amizade).Result;

            if (jaEAmigo != null)
            {
                return BadRequest("O Usuario requisitado já esta na sua lista de amigos");
            }

            string mensagem = $"Olá {solicitado.Nome.Split(" ")[0]}, gostaria de ser meu amigo aqui na {ConfigUsuario.NomeRedeSocial}. Seria ótimo trocar ideias e experiências.";

            var notificacao = new Notificacao(usuario.Id, solicitado.Id, 1, mensagem);
            var solicitacaoDoAmigo = new Notificacao(solicitado.Id, usuario.Id, 1, mensagem);
            // Tipo 1, solicitação de amizade

            var jaExisteSolicitacao = _ntfRepository.JaFoiNotificado(notificacao).Result;
            var jaExisteSolicitacaoDoAmigo = _ntfRepository.JaFoiNotificado(solicitacaoDoAmigo).Result;

            if (jaExisteSolicitacao != null)
            {
                return BadRequest("Solicitação de Amizade já Enviada");
            }

            if (jaExisteSolicitacaoDoAmigo != null)
            {
                return BadRequest("Solicitação do Amigo em Questão Pendente");
            }

            _ntfRepository.Add(notificacao);

            // Incrementa as solicitações de amizade do amigo
            homeAmigo.NumSolicitacoesAmizade++;
            _infHomeRepository.Update(homeAmigo);

            // ******* Area destinada aos ganhos de XP automatico da Rede ****** //
            Calculos c = new();

            c.GanharXP(100, ref usuario);

            _usrRepository.Update(usuario);
            // ******* Area destinada aos ganhos de XP automatico da Rede ****** //

            var resposta = new NotificacoesDoUsuarioDto(notificacao.Remetente, notificacao.TipoDeNotificacao, notificacao.MensagemDaNotificacao);

            return Ok(resposta);
        }

        // Aceitar solicitação de amizade
        [HttpPost]
        [Route("Amizades/Aceitar/{idDeQuemPediu}")]
        public IActionResult AceitarSoliAmiza(Guid idDeQuemPediu)
        {
            // Usuarios 
            var usuario = ConfigUsuario.CurrentUser;

            if (usuario is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            var quemPediu = _usrRepository.Get(idDeQuemPediu).Result;

            if (quemPediu is null)
            {
                return NotFound("Usuario que solicitou não encontrado");
            }

            // Home do amigo atualizada com a notificação
            var home = _infHomeRepository.GetViaUsr(usuario.Id).Result;
            var homeAmigo = _infHomeRepository.GetViaUsr(idDeQuemPediu).Result;

            var pedidoExistente = _ntfRepository.JaFoiNotificado(
                new Notificacao
                (
                    quemPediu.Id,
                    usuario.Id,
                    1,
                    ""
                )).Result;

            if (pedidoExistente is null)
            {
                return NotFound("Não Existe nenhum Pedido de Amizade pra Aceitar");
            }

            if (home is null)
            {
                return Problem("Informações da Home não encontradas");
            }

            if (homeAmigo is null)
            {
                return Problem("Informações da Home do Amigo não encontradas");
            }

            string mensagem = $"{quemPediu.Nome.Split(" ")[0]}, agora somos amigos aqui na {ConfigUsuario.NomeRedeSocial}.";

            var notificacao = new Notificacao(usuario.Id, quemPediu.Id, 3, mensagem);
            // Tipo 3, Resposta

            var jaExiste = _ntfRepository.JaFoiNotificado(notificacao).Result;

            if (jaExiste != null)
            {
                return BadRequest("Solicitação já aceita");
            }

            _ntfRepository.Add(notificacao);

            home.NumSolicitacoesAmizade--;
            home.NumAmigos++;
            _infHomeRepository.Update(home);

            homeAmigo.NumNotificacoesNaoLidas++;
            homeAmigo.NumAmigos++;
            _infHomeRepository.Update(homeAmigo);

            Amizade amizade = new(quemPediu.Id, usuario.Id);

            _amzdRepository.Add(amizade);
            _ntfRepository.Delete(pedidoExistente);

            // ******* Area destinada aos ganhos de XP automatico da Rede ****** //
            Calculos c = new();

            c.GanharXP(300, ref usuario);

            _usrRepository.Update(usuario);
            // ******* Area destinada aos ganhos de XP automatico da Rede ****** //

            var resposta = new NotificacoesDoUsuarioDto(notificacao.Remetente, notificacao.TipoDeNotificacao, notificacao.MensagemDaNotificacao);

            return Ok(resposta);
        }

        // Aceitar solicitação de amizade
        [HttpPost]
        [Route("Amizades/Recusar/{idDeQuemPediu}")]
        public IActionResult RecusarSoliAmiza(Guid idDeQuemPediu)
        {
            // Usuarios 
            var usuario = ConfigUsuario.CurrentUser;

            if (usuario is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            var quemPediu = _usrRepository.Get(idDeQuemPediu).Result;

            if (quemPediu is null)
            {
                return NotFound("Usuario que solicitou não encontrado");
            }

            // Home do amigo atualizada com a notificação
            var home = _infHomeRepository.GetViaUsr(usuario.Id).Result;
            var homeAmigo = _infHomeRepository.GetViaUsr(idDeQuemPediu).Result;

            var pedidoExistente = _ntfRepository
                .JaFoiNotificado(new Notificacao
                (
                    quemPediu.Id,
                    usuario.Id,
                    1,
                    ""
                )).Result;

            if (pedidoExistente is null)
            {
                return NotFound("Não Existe nenhum Pedido de Amizade pra Recusar");
            }

            if (home is null)
            {
                return Problem("Informações da Home não encontradas");
            }

            if (homeAmigo is null)
            {
                return Problem("Informações da Home do Amigo não encontradas");
            }

            string mensagem = $"{quemPediu.Nome.Split(" ")[0]}, infelizmente não posso aceitar seu pedido de amizade aqui na {ConfigUsuario.NomeRedeSocial}.";

            var notificacao = new Notificacao(usuario.Id, quemPediu.Id, 3, mensagem);
            // Tipo 3, Resposta

            var jaExiste = _ntfRepository.JaFoiNotificado(notificacao).Result;

            if (jaExiste != null)
            {
                return BadRequest("Solicitação já recusada");
            }

            _ntfRepository.Add(notificacao);

            home.NumSolicitacoesAmizade--;
            _infHomeRepository.Update(home);

            homeAmigo.NumNotificacoesNaoLidas++;
            _infHomeRepository.Update(homeAmigo);

            Amizade amizade = new(quemPediu.Id, usuario.Id);

            var jaEAmigo = _amzdRepository.VerificarAmizade(amizade).Result;

            if (jaEAmigo != null)
            {
                return BadRequest("O Usuario requisitado já esta na sua lista de amigos");
            }

            _ntfRepository.Delete(pedidoExistente);

            var resposta = new NotificacoesDoUsuarioDto(notificacao.Remetente, notificacao.TipoDeNotificacao, notificacao.MensagemDaNotificacao);

            return Ok(resposta);
        }

        // Desfazer amizades
        [HttpDelete]
        [Route("Amizades/Desfazer/{idAmigo}")]
        public IActionResult DesfazerAmiza(Guid idAmigo)
        {
            var usuario = ConfigUsuario.CurrentUser;
            var amigo = _usrRepository.Get(idAmigo).Result;

            if (usuario is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            if (amigo is null)
            {
                return NotFound("Usuario Não Encontrado");
            }

            var amizade = _amzdRepository.VerificarAmizade(new Amizade(usuario.Id, idAmigo)).Result;

            if (amizade is null)
            {
                return BadRequest("Os usuários não são amigos");
            }

            var usrHome = _infHomeRepository.GetViaUsr(usuario.Id).Result;
            var amigoHome = _infHomeRepository.GetViaUsr(amigo.Id).Result;

            if (usrHome is null)
            {
                return Problem("Home do usuário não encontrada");
            }

            if (amigoHome is null)
            {
                return Problem("Home do amigo não encontrada");
            }

            usrHome.NumAmigos--;
            amigoHome.NumAmigos--;

            _infHomeRepository.Update(usrHome);
            _infHomeRepository.Update(amigoHome);
            _amzdRepository.Delete(amizade);

            return NoContent();
        }

        // Usuario/Home:
        // Carregar a Home 
        [HttpGet]
        [Route("Home")]
        public IActionResult GetHomeUsr()
        {
            var usuario = ConfigUsuario.CurrentUser;

            if (usuario is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            var homeUsuario = _infHomeRepository.GetViaUsr(usuario.Id).Result;

            if (homeUsuario is null)
            {
                return Problem("Home do Usuario Não Encontrada");
            }

            var resposta = new InformHomeDto
                (
                    homeUsuario.IdUsuario,
                    usuario.Nome,
                    homeUsuario.NumNotificacoesNaoLidas,
                    homeUsuario.NumSolicitacoesAmizade,
                    homeUsuario.NumAmigos,
                    homeUsuario.NumMensagensNaoLidas
                 );

            return Ok(resposta);
        }

        // Usuario/Login:
        // Efetuar Login 
        [HttpGet]
        [Route("Login/{email}/{senha}")]
        public IActionResult Login(string email, string senha)
        {
            var usuario = _usrRepository.GetViaLogin(email, senha).Result;

            if (usuario is null)
            {
                return NotFound("Email ou Senha Errados");
            }

            ConfigUsuario.CurrentUser = usuario;

            var token = TokenService.GenerateToken(usuario);

            return Ok(token);
        }

        // Deslogar da Rede
        [HttpPost]
        [Route("Login/Deslogar")]
        public IActionResult LoginOff()
        {
            if (ConfigUsuario.CurrentUser is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            ConfigUsuario.CurrentUser = null;

            return Ok();
        }

        // Rota para alterar as informações do Login
        [HttpPut]
        [Route("Login/{email}/{senha}")]
        public IActionResult LoginUpdate(string email, string senha)
        {
            var usuario = ConfigUsuario.CurrentUser;

            if (usuario is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            // Lógica pra impedir a troca pra email existente
            var usuarioSemelhante = _usrRepository.GetSemelhanteEmail(email).Result;

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

        // Usuario/Notificacoes:
        // Carregar as Notificacoes 
        [HttpGet]
        [Route("Notificacoes")]
        public IActionResult GetNotiUsr()
        {
            var usuario = ConfigUsuario.CurrentUser;

            if (usuario is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            var notiUsuario = _ntfRepository.GetAllUsr(usuario.Id).Result;
            var homeUsr = _infHomeRepository.GetViaUsr(usuario.Id).Result;

            if (notiUsuario is null || notiUsuario.Count == 0)
            {
                return NotFound("Nenhuma Notificação Existente");
            }

            if (homeUsr is null)
            {
                return Problem("Informações da Home não Encontradas");
            }

            homeUsr.NumNotificacoesNaoLidas = 0;
            _infHomeRepository.Update(homeUsr);

            return Ok(notiUsuario);
        }

        // Carregar os Pedidos de Amizade 
        [HttpGet]
        [Route("Notificacoes/PedidoAmizade")]
        public IActionResult GetNotiUsrAmizade()
        {
            var usuario = ConfigUsuario.CurrentUser;

            if (usuario is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            var notiUsuario = _ntfRepository.GetAllNotiAmizadeUsr(usuario.Id).Result;

            if (notiUsuario is null || notiUsuario.Count == 0)
            {
                return NotFound("Nenhum Pedido de Amizade Pendente");
            }

            return Ok(notiUsuario);
        }

        // Usuario/Perfil:
        // Rota para carregar o perfil do usuario *
        [HttpGet]
        [Route("Perfil")]
        public IActionResult PerfilUsr()
        {
            var usuario = ConfigUsuario.CurrentUser;

            Calculos c = new();

            if (usuario is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            var estatisticas = _estRepository.Get(usuario.IdEstatisticas).Result;

            if (estatisticas is null)
            {
                if (usuario.Peso != 0 && usuario.Altura != 0)
                {
                    Estatisticas novasEst = new(c.CalcularIMC(usuario), c.CalcularAguaDiaria(true, usuario));
                    var ultimaEstatistica = _estRepository.GetLast().Result;

                    // Código pra substituir o autoIncrement do Banco
                    if (ultimaEstatistica != null)
                    {
                        novasEst.Id = ultimaEstatistica.Id + 1;
                    }
                    else
                    {
                        novasEst.Id = 1;
                    }
                    // Visa manter o uso de id int ao invés de trocar pra Guid

                    _estRepository.Add(novasEst);

                    ultimaEstatistica = _estRepository.GetLast().Result;
                    if (ultimaEstatistica != null)
                    {
                        usuario.IdEstatisticas = ultimaEstatistica.Id;
                        _usrRepository.Update(usuario);
                    }
                    var resposta = new PerfilUsuarioDto
                        (usuario.Nome, usuario.Altura, usuario.Peso,
                        novasEst.IMC, novasEst.AguaDiaria, usuario.Level,
                        usuario.XP, usuario.ImagemPerfil);
                    return Ok(resposta);
                }
                else
                {
                    return BadRequest("Não é possivel gerar o Perfil sem Peso e Altura");
                }
            }
            else
            {
                var resposta = new PerfilUsuarioDto
                    (usuario.Nome, usuario.Altura, usuario.Peso,
                    estatisticas.IMC, estatisticas.AguaDiaria, usuario.Level,
                    usuario.XP, usuario.ImagemPerfil);
                return Ok(resposta);
            }
        }

        // Rota pra atualizar a foto do Perfil
        [HttpPost]
        [Route("Perfil/Foto/{imagemBase64}")]
        public IActionResult PerfilUsrImagem(string imagemBase64)
        {
            var usuario = ConfigUsuario.CurrentUser;

            if (usuario is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            usuario.ImagemPerfil = imagemBase64;

            _usrRepository.Update(usuario);

            return Ok();
        }

        // Rota pra pegar apenas a foto do Perfil
        [HttpGet]
        [Route("Perfil/Foto")]
        public IActionResult GetPerfilUsrImagem()
        {
            var usuario = ConfigUsuario.CurrentUser;

            if (usuario is null)
            {
                return BadRequest("Nenhum Usuario Logado");
            }

            return Ok(usuario.ImagemPerfil);
        }

        // Usuario/Recuperacao:
        // Recuperar senha simples
        [HttpGet]
        [Route("Recuperacao/{email}/{palavraRecuperacao}")]
        public IActionResult RecuperarSenha(string email, string palavraRecuperacao)
        {
            var usuario = _usrRepository.RecuperarSenha(email, palavraRecuperacao).Result;

            if (usuario is null)
            {
                return NotFound("Email ou Palavra de Recuperação Errados");
            }

            string caracteresPermitidos = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+-=[]{}|;:,.<>?";

            StringBuilder sb = new();
            Random random = new();

            for (int i = 0; i < 10; i++)
            {
                int index = random.Next(caracteresPermitidos.Length);
                sb.Append(caracteresPermitidos[index]);
            }

            string senhaAleatoria = sb.ToString();

            string resposta = $"{usuario.Nome}, agradecemos por usar a rede {ConfigUsuario.NomeRedeSocial}, sua nova senha temporaria pra acesso é: {senhaAleatoria}";

            usuario.Senha = senhaAleatoria;
            _usrRepository.Update(usuario);

            return Ok(resposta);
        }

        // Usuario/Todos:
        // Retornar todos os Usuarios 
        [HttpGet]
        [Route("Todos")]
        public IActionResult GetAllUsr()
        {
            var usuarios = _usrRepository.GetAll().Result;

            if (usuarios is null || usuarios.Count == 0)
            {
                return NotFound();
            }

            return Ok(usuarios);
        }
    }
}
