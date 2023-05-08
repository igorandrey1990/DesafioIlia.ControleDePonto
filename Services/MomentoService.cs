using DesafioIlha.ControleDePonto.DAL;
using DesafioIlha.ControleDePonto.Models;
using DesafioIlha.ControleDePonto.Services.Interfaces;

namespace DesafioIlha.ControleDePonto.Services
{
    public class MomentoService : IMomentoService
    {
        public readonly MomentoContext _momentoContext;

        public MomentoService(MomentoContext momentoContext)
        {
            _momentoContext = momentoContext;
        }

        public MomentoService() { }

        public List<Momento> GetMomentos()
        {
            return _momentoContext.Momentos.ToList();
        }
        public Momento BaterPonto(Momento p_momento, out Mensagem msg)
        {
            if (ValidarDataHora(p_momento, out msg))
                return null;
            if (ValidarMomentosDiarios(p_momento, out msg))
                return null;
            if (ValidarHorarioAlmoco(p_momento, out msg))
                return null;
            if (ValidarDiaDaSemana(p_momento, out msg))
                return null;
            if (ValidarSeHorarioExiste(p_momento, out msg))
                return null;

            _momentoContext.Momentos.Add(p_momento);
            _momentoContext.SaveChangesAsync();
            return p_momento;
        }

        #region Validações
        private bool ValidarDataHora(Momento momento, out Mensagem msg)
        {
            msg = new Mensagem();

            if (momento.dataHora == new DateTime())
            {
                msg.mensagem = "Campo obrigatório não informado";
                msg.statusCode = 400;
                return true;
            }
            return false;
        }
        private bool ValidarMomentosDiarios(Momento momento, out Mensagem msg)
        {
            msg = new Mensagem();
            var dateTime = momento.dataHora.ToString("dd/MM/yyyy");
            var momentos = from moments in this.GetMomentos()
                           where moments.dataHora.ToString().Substring(0, 10) == dateTime
                           select moments;

            if (momentos.Count() >= 4)
            {
                msg.mensagem = "Apenas 4 horários podem ser registrados por dia";
                msg.statusCode = 403;
                return true;
            }

            return false;
        }

        private bool ValidarHorarioAlmoco(Momento momento, out Mensagem msg)
        {
            msg = new Mensagem();
            var dateTime = momento.dataHora.ToString("dd/MM/yyyy");
            var momentos = from moments in this.GetMomentos()
                           where moments.dataHora.ToString().Substring(0, 10) == dateTime
                           select moments;

            if (momentos.Count() == 2)
            {
                var ultimo = momentos.OrderByDescending(m => m.dataHora).First();

                if (momento.dataHora.Subtract(ultimo.dataHora).TotalMinutes < 60)
                {
                    msg.mensagem = "Deve haver no mínimo 1 hora de almoço";
                    msg.statusCode = 403;
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
                msg.statusCode = 403;
                return true;
            }

            return false;
        }

        private bool ValidarSeHorarioExiste(Momento momento, out Mensagem msg)
        {
            msg = new Mensagem();
            var dateTime = momento.dataHora.ToString("dd/MM/yyyy HH:mm");
            var momentos = from moments in this.GetMomentos()
                           where moments.dataHora.ToString().Substring(0, 16) == dateTime
                           select moments;

            if (momentos.Any())
            {
                msg.mensagem = "Horário já registrado";
                msg.statusCode = 409;
                return true;
            }

            return false;
        }
        #endregion

    }
}
