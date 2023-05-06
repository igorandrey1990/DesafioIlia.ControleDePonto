using DesafioIlha.ControleDePonto.Models;
using DesafioIlha.ControleDePonto.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DesafioIlha.ControleDePonto.Controllers
{
    /// <summary>
    /// Controller que fornece funcionalidade para gerar e manter relatorios.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FolhasDePontoController : ControllerBase
    {
        private readonly IFolhaDePontoService _folhaDePontoService;
        private readonly ILogger<FolhasDePontoController> _logger;

        public FolhasDePontoController(IFolhaDePontoService folhaDePontoService, ILogger<FolhasDePontoController> logger)
        {
            _folhaDePontoService = folhaDePontoService;
            _logger = logger;
        }

        /// <summary>
        /// Gera relatorio para determinado mes e ano.
        /// </summary>
        /// <returns>A List of <see cref="Contact"/></returns>
        /// <response code="200">Retorna OK</response>
        /// <response code="400">Se a request estiver incorreta</response>
        [HttpGet("{anoMes}")]
        public ActionResult<Relatorio> GerarRelatorio(string anoMes)
        {
            _logger.LogInformation("Chamada a GetRelatorio");
            return _folhaDePontoService.GetRelatorio(anoMes);
        }
    }
}
