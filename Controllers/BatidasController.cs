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

        public BatidasController(IMomentoService momentoInterface)
        {
            _momentoInterface = momentoInterface;
        }

        /// <summary>
        /// Bater Ponto
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Momento>> BaterPonto()
        {
            Momento momento = new Momento();
            Mensagem msg = new Mensagem();
            DateTime dataHora;
            momento.dataHora = DateTime.Now;

            if (ValidarDataHora(momento, out msg))
                return StatusCode(400, msg.mensagem);
            if (ValidarMomentosDiarios(momento, out msg))
                return StatusCode(403, msg.mensagem);
            if (ValidarHorarioAlmoco(momento, out msg))
                return StatusCode(403, msg.mensagem);
            if (ValidarDiaDaSemana(momento, out msg))
                return StatusCode(403, msg.mensagem);
            if (ValidarSeHorarioExiste(momento, out msg))
                return StatusCode(409, msg.mensagem);

            await _momentoInterface.BaterPonto(momento);

            return CreatedAtAction(nameof(BaterPonto), new { id = momento.Id }, momento);
        }

        #region Validações
        private bool ValidarDataHora(Momento momento, out Mensagem msg)
        {
            msg = new Mensagem();

            if (DateTime.TryParse(momento.dataHora.ToString(), out DateTime date))
            {
                msg.mensagem = "Data e hora em formato inválido";
                return true;
            }
            else if (momento.dataHora.ToString().IsNullOrEmpty())
            {
                msg.mensagem = "Campo obrigatório não informado";
                return true;
            }

            return false;
        }
        private bool ValidarMomentosDiarios(Momento momento, out Mensagem msg)
        {
            msg = new Mensagem();
            var dateTime = momento.dataHora.ToString("dd/MM/yyyy");
            var momentos = from moments in _momentoInterface.GetMomentos()
                           where moments.dataHora.ToString().Substring(0, 10) == dateTime
                           select moments;

            if (momentos.Count() >= 4)
            {
                msg.mensagem = "Apenas 4 horários podem ser registrados por dia";
                return true;
            }

            return false;
        }

        private bool ValidarHorarioAlmoco(Momento momento, out Mensagem msg)
        {
            msg = new Mensagem();
            var dateTime = momento.dataHora.ToString("dd/MM/yyyy");
            var momentos = from moments in _momentoInterface.GetMomentos()
                           where moments.dataHora.ToString().Substring(0, 10) == dateTime
                           select moments;

            if (momentos.Count() == 2)
            {
                var ultimo = momentos.OrderByDescending(m => m.dataHora).First();

                if (momento.dataHora.Subtract(ultimo.dataHora).TotalMinutes < 60)
                {
                    msg.mensagem = "Deve haver no mínimo 1 hora de almoço";
                    return true;
                }
            }

            return false;
        }

        private bool ValidarDiaDaSemana(Momento momento, out Mensagem msg)
        {
            msg = new Mensagem();
            if (momento.dataHora.DayOfWeek.Equals(DayOfWeek.Sunday) || momento.dataHora.DayOfWeek.Equals(DayOfWeek.Saturday))
            {
                msg.mensagem = "Sábado e domingo não são permitidos como dia de trabalho";
                return true;
            }

            return false;
        }

        private bool ValidarSeHorarioExiste(Momento momento, out Mensagem msg)
        {
            msg = new Mensagem();
            var dateTime = momento.dataHora.ToString("yyyy-MM-dd HH:mm");
            var momentos = from moments in _momentoInterface.GetMomentos()
                           where moments.dataHora.ToString().Substring(0, 16) == dateTime
                           select moments;

            if (momentos.Any())
            {
                msg.mensagem = "Horário já registrado";
                return true;
            }

            return false;
        }
        #endregion

    }
}
