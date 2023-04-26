using DesafioIlha.ControleDePonto.Models;

namespace DesafioIlha.ControleDePonto.Services.Interfaces
{
    public interface IMomentoService
    {
        List<Momento> GetMomentos();
        Task<Momento> BaterPonto(Momento momento);
    }
}
