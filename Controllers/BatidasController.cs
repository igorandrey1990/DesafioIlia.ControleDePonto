using DesafioIlha.ControleDePonto.DAL;
using DesafioIlha.ControleDePonto.Models;
using DesafioIlha.ControleDePonto.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.Net;

namespace DesafioIlha.ControleDePonto.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class BatidasController : ControllerBase
    {
        private readonly IMomentoService _momentoInterface;
        private Mensagem msg = new Mensagem();

       

        /// <summary>
        /// Bater Ponto
        /// </summary>
        [HttpPost]
        public ActionResult<Momento> BaterPonto(Momento p_momento)
        {

            var ponto = _momentoInterface.BaterPonto(p_momento, out msg);

            return ponto == null ? new CreatedResult("", 0) { StatusCode = msg.statusCode, Value = msg.mensagem } : CreatedAtAction(nameof(BaterPonto), new { id = p_momento.Id }, p_momento);
        }
    }
}
