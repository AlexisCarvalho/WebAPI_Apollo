using Microsoft.AspNetCore.Mvc;
using WebAPI_Apollo.Application.Services;

namespace WebAPI_Apollo.Controllers
{
    [ApiController]
    [Route("api/Auth")]
    public class Auth : Controller
    {
        // (Deixei um usuario padrão causo queira testar a rota Auth)
        // Se não teria que saber quem tem conta no banco e taus.
        // A obrigatoridade de usar isso ta desativada, se quiser
        // pode fingir que não existe que ta sussa

        //  Email: Alexis@gmail.com Senha: 123456)
        [HttpPost]
        public IActionResult Authentication(string email, string senha)
        {
            if (email == "Alexis@gmail.com" && senha == "123456")
            {
                var token = TokenService.GenerateToken(new Domain.Model.Usuario());
                return Ok(token);
            }
            return BadRequest("Email ou Senha Invalidos");
        }
    }
}
