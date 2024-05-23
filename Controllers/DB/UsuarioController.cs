using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebAPI_Apollo.Infraestrutura.Services;
using WebAPI_Apollo.Infraestrutura.Services.Repository.DB;
using WebAPI_Apollo.Model;
using WebAPI_Apollo.Model.DTOs;
using WebAPI_Apollo.Model.ViewModel;

namespace WebAPI_Apollo.Controllers.DB
{
    [ApiController]
    [Route("Usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usrRepository;
        private readonly IEstatisticasRepository _estRepository;

        public UsuarioController(IUsuarioRepository usrRepository, IEstatisticasRepository estRepository)
        {
            _usrRepository = usrRepository ?? throw new ArgumentNullException();
            _estRepository = estRepository ?? throw new ArgumentNullException();
        }

        // Adicionar um Usuario
        //[Authorize]
        //[Obsolete("Esta rota está obsoleta. Atualizar baseado no RAM.")]
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

            _usrRepository.Add(novoUsuario);

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

                        _estRepository.Add(est);

                        var ultimaEstatistica = _estRepository.GetLast();
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

                        EstatisticasRepository doisEstRepository = new();
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

                    _estRepository.Add(est);

                    var ultimaEstatistica = _estRepository.GetLast();
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

            if (usuario is null)
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

            string resposta = $"{usuario.Nome}, agradecemos por usar a rede {ConfSistema.NomeRedeSocial}, sua nova senha temporária pra acesso é: {senhaAleatoria}";

            usuario.Senha = senhaAleatoria;
            _usrRepository.Update(usuario);

            return Ok(resposta);
        }
    }
}
