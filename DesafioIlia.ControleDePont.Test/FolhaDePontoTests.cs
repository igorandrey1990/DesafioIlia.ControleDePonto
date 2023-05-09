using DesafioIlha.ControleDePonto.DAL;
using DesafioIlha.ControleDePonto.Models;
using DesafioIlha.ControleDePonto.Services;
using Microsoft.EntityFrameworkCore;
using NuGet.Frameworks;
using System;

namespace DesafioIlia.ControleDePont.Test
{
    public class FolhaDePontoTests
    {
        private readonly MomentoContext context;

        public FolhaDePontoTests()
        {
            DbContextOptionsBuilder dbOptions = new DbContextOptionsBuilder()
                    .UseInMemoryDatabase(
                        Guid.NewGuid().ToString()
                    );

            context = new MomentoContext(dbOptions.Options);
        }

        [Fact]
        public void GerarRelatorio_ShouldPass()
        {
            context.Momentos.AddRange(
                new Momento() { Id = 1, dataHora = DateTime.Parse("2023-04-26 08:00:00.0000000") },
                new Momento() { Id = 2, dataHora = DateTime.Parse("2023-04-26 12:00:00.0000000") },
                new Momento() { Id = 3, dataHora = DateTime.Parse("2023-04-26 13:00:00.0000000") },
                new Momento() { Id = 4, dataHora = DateTime.Parse("2023-04-26 18:00:00.0000000") }
                );

            context.SaveChanges();

            var service = new FolhaDePontoService(context);

            var relatorio = service.GetRelatorio("2023/04");

            Assert.Equal("2023/04", relatorio.mes);
            Assert.Equal("PT159H", relatorio.horasDevidas);
            Assert.Equal("PT0", relatorio.horasExcedentes);
            Assert.Equal("PT9H", relatorio.horasTrabalhadas);
        }
    }
}