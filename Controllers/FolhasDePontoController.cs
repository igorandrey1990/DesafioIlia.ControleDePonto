using DesafioIlha.ControleDePonto.Models;
using DesafioIlha.ControleDePonto.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DesafioIlha.ControleDePonto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolhasDePontoController : ControllerBase
    {
        private readonly IFolhaDePontoService _folhaDePontoService;

        public FolhasDePontoController(IFolhaDePontoService folhaDePontoService)
        {
            _folhaDePontoService = folhaDePontoService;
        }

        /// <summary>
        /// Gerar Relatório
        /// </summary>
        [HttpGet("{anoMes}")]
        public async Task<ActionResult<Relatorio>> GerarRelatorio(string anoMes)
        {
            Relatorio relatorio = _folhaDePontoService.GetRelatorio(anoMes);
            return relatorio;
        }
    }
}
