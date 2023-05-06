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

        public List<Momento> GetMomentos() {
            return _momentoContext.Momentos.ToList();
        }
        public async Task<Momento> BaterPonto(Momento momento)
        {
            _momentoContext.Momentos.Add(momento);
            await _momentoContext.SaveChangesAsync();
            return momento;
        }

        
    }
}
