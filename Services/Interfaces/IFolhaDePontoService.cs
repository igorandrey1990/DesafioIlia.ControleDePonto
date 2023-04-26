using DesafioIlha.ControleDePonto.Models;

namespace DesafioIlha.ControleDePonto.Services.Interfaces
{
    public interface IFolhaDePontoService
    {
        Relatorio GetRelatorio(string anoMes);
    }
}
