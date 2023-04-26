using DesafioIlha.ControleDePonto.DAL;
using DesafioIlha.ControleDePonto.Models;
using DesafioIlha.ControleDePonto.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Text;
using static Azure.Core.HttpHeader;

namespace DesafioIlha.ControleDePonto.Services
{
    public class FolhaDePontoService : IFolhaDePontoService
    {
        public readonly MomentoContext _momentoContext;

        public FolhaDePontoService(MomentoContext momentoContext)
        {
            _momentoContext = momentoContext;
        }

        public Relatorio GetRelatorio(string anoMes)
        {
            string horasTrabalhadas, horasExcedentes, horasDevidas;

            List<Registro> registros = CriarRegistrosMes(anoMes);

            CalcularHoras(registros, out horasTrabalhadas, out horasExcedentes, out horasDevidas);

            return new Relatorio()
            {
                mes = anoMes,
                horasTrabalhadas = horasTrabalhadas.ToString(),
                horasExcedentes = horasExcedentes.ToString(),
                horasDevidas = horasDevidas.ToString(),
                registros = registros
            };
        }

        private List<Registro> CriarRegistrosMes(string anoMes)
        {
            IQueryable<Momento> momentos = from m in _momentoContext.Momentos
                                           where m.dataHora.ToString().Substring(0, 7) == anoMes
                                           orderby m.dataHora ascending
                                           select m;

            List<Registro> registrosMes = new List<Registro>();
            Registro registro = new Registro();

            foreach (Momento momento in momentos)
            {
                if (registro.dia == null)
                {
                    registro.dia = momento.dataHora.Day.ToString();
                    registro.horarios.Add(momento.dataHora.ToShortTimeString());
                }
                else if (registro.dia == momento.dataHora.Day.ToString())
                    registro.horarios.Add(momento.dataHora.ToShortTimeString());
                else if (registro.dia != momento.dataHora.Day.ToString())
                {
                    registro = new Registro();
                    registro.dia = momento.dataHora.Day.ToString();
                    registro.horarios.Add(momento.dataHora.ToShortTimeString());
                }

                if (IsComplete(momentos, registro.dia, registro))
                    registrosMes.Add(registro);
            }

            return registrosMes;
        }

        private bool IsComplete(IQueryable<Momento> momentos, string dia, Registro registro)
        {
            return momentos.Where(a => a.dataHora.Day.ToString() == dia).Count() == registro.horarios.Count ? true : false;
        }

        private void CalcularHoras(List<Registro> registrosMes, out string horasTrabalhadas, out string horasExcedentes, out string horasDevidas)
        {
            double tempoTotal = 0;
            horasTrabalhadas = CalcularHorasTrabalhadas(registrosMes, out tempoTotal);
            horasExcedentes = CalcularHorasExcedentes(tempoTotal);
            horasDevidas = CalcularHorasDevidas(tempoTotal);
        }

        private string CalcularHorasTrabalhadas(List<Registro> registrosMes, out double tempoTotal)
        {
            double horasT = 0;
            foreach (Registro registro in registrosMes)
            {
                if (registro.horarios.Count == 3)
                    registro.horarios.Add(TimeOnly.Parse(registro.horarios.Last()).AddHours(1).ToString());

                horasT += DateTime.Parse(registro.horarios[1]).Subtract(DateTime.Parse(registro.horarios[0])).TotalSeconds;
                horasT += DateTime.Parse(registro.horarios[3]).Subtract(DateTime.Parse(registro.horarios[2])).TotalSeconds;
            }

            tempoTotal = horasT;

            return FormatTimeString(TimeSpan.FromSeconds(horasT));
        }

        private string FormatTimeString(TimeSpan horasTrabalhadas)
        {
            var time = String.Format("{0}:{1}:{2}", (int)horasTrabalhadas.TotalHours, horasTrabalhadas.Minutes, horasTrabalhadas.Seconds);
            var array = time.Split(':');
            StringBuilder str = new StringBuilder();
            str.Append("PT");

            if (array[0] != "0")
                str.Append(array[0] + 'H');

            if (array[1] != "0")
                str.Append(array[0] + 'M');

            if (array[1] != "0")
                str.Append(array[0] + 'S');

            return str.ToString();
        }

        private string CalcularHorasExcedentes(double horasTrabalhadas)
        {
            var horasExcedentes = (TimeSpan.FromSeconds(horasTrabalhadas) - TimeSpan.FromHours(168));

            if (horasExcedentes.TotalSeconds > 0)
                return FormatTimeString(horasExcedentes);
            else
                return "PT0";
        }

        private string CalcularHorasDevidas(double horasTrabalhadas)
        {
            var horasDevidas = (TimeSpan.FromHours(168) - TimeSpan.FromSeconds(horasTrabalhadas));

            if (horasDevidas.TotalSeconds > 0)
                return FormatTimeString(horasDevidas);
            else
                return "PT0";
        }
    }
}
