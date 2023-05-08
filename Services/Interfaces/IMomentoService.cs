using DesafioIlha.ControleDePonto.Models;

namespace DesafioIlha.ControleDePonto.Services.Interfaces
{
    public interface IMomentoService
    {
        List<Momento> GetMomentos();
        Momento BaterPonto(Momento momento, out Mensagem msg);
    }
}
