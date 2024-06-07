using Microsoft.AspNetCore.Mvc;
using WebAPI_Apollo.Application.Services;
using WebAPI_Apollo.Domain.Model.Interfaces;

namespace WebAPI_Apollo.Controllers
{
    [ApiController]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        // (Deixei um usuario padrão causo queira testar a rota Auth)
        // Se não teria que saber quem tem conta no banco e taus.
        // A obrigatoridade de usar isso ta desativada, se quiser
        // pode fingir que não existe que ta sussa

        //  Email: Alexis@gmail.com Senha: 123456)

        private readonly IUsuarioRepository _usrRepository;

        public AuthController(IUsuarioRepository usrRepository)
        {
            _usrRepository = usrRepository ?? throw new ArgumentNullException();
        }


        [HttpPost]
        public IActionResult Authentication(string email, string senha)
        {
            var usuario = _usrRepository.GetViaLogin(email, senha);

            if (usuario is null)
            {
                return NotFound("Email ou Senha Errados");
            }

            var token = TokenService.GenerateToken(usuario);
            return Ok(token);
        }

        [HttpGet]
        public IActionResult IdViaToken(string token)
        {
            var id = TokenService.GetUserIdFromToken(token);
            return Ok(id);
        }
    }
}
