using Microsoft.AspNetCore.Mvc;
using WebAPI_Apollo.Infraestrutura.Services;

namespace WebAPI_Apollo.Controllers
{
    //[ApiController]
    //[Route("Auth")]
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
                var token = TokenService.GenerateToken(new Model.Usuario());
                // Trocar pelo Usuario encontrado no futuro
                return Ok(token);
            }
            return BadRequest("Email ou Senha Invalidos");
        }
    }
}
