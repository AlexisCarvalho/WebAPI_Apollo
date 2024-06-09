using Microsoft.AspNetCore.Mvc;
using WebAPI_Apollo.Application.Services;
using WebAPI_Apollo.Infraestrutura;

namespace WebAPI_Apollo.Controllers
{
    [ApiController]
    [Route("api/Config")]
    public class ConfigController : ControllerBase
    {
        private readonly ConfigService _configService;

        public ConfigController(ConfigService configService)
        {
            _configService = configService;
        }

        [HttpPost("AtivarBanco")]
        public IActionResult ToggleDB([FromBody] bool dbAtivado)
        {
            if (!ConfigService.DBFuncionando)
            {
                TestarBDService.ExecutarTeste();
            }

            if (!ConfigService.DBFuncionando)
            {
                return Problem("O Banco de Dados Não Esta Passando nos Testes");
            }

            _configService.DBAtivado = dbAtivado;
            return Ok($"Banco Ativado: {dbAtivado}");
        }
    }
}
