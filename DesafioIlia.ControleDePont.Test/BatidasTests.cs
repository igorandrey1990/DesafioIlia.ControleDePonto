using DesafioIlha.ControleDePonto.Controllers;
using DesafioIlha.ControleDePonto.DAL;
using DesafioIlha.ControleDePonto.Models;
using DesafioIlha.ControleDePonto.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioIlia.ControleDePont.Test
{
    public class BatidasTests
    {
        private readonly MomentoContext context;

        public BatidasTests()
        {
            DbContextOptionsBuilder dbOptions = new DbContextOptionsBuilder()
                    .UseInMemoryDatabase(
                        Guid.NewGuid().ToString()
                    );

            context = new MomentoContext(dbOptions.Options);
        }

        [Fact]
        public void BatidaPonto_ShoudFail_NullDate_Error400()
        {
            var service = new MomentoService(context);

            Momento momento = new Momento();
            Mensagem msg = new Mensagem();

            var result = service.BaterPonto(momento, out msg);

            Assert.Null(result);
            Assert.Equal(400, msg.statusCode);
            Assert.Equal("Campo obrigatório não informado", msg.mensagem);

        }

        [Fact]
        public void BatidaPonto_ShouldFail_TimeAlreadyInserted_Error409()
        {
            context.Momentos.AddRange(
                new Momento() { Id = 1, dataHora = DateTime.Parse("2023-04-26 08:00:00.0000000") }
                //new Momento() { Id = 2, dataHora = DateTime.Parse("2023-04-26 12:00:00.0000000") },
                //new Momento() { Id = 3, dataHora = DateTime.Parse("2023-04-26 13:00:00.0000000") },
                //new Momento() { Id = 4, dataHora = DateTime.Parse("2023-04-26 18:00:00.0000000") }
                );

            context.SaveChanges();

            var service = new MomentoService(context);

            Mensagem msg = new Mensagem();
            Momento momento = new Momento()
            {
                dataHora = DateTime.Parse("2023-04-26 08:00:00.0000000")
            };

            var result = service.BaterPonto(momento, out msg);

            Assert.Null(result);
            Assert.Equal(409, msg.statusCode);
            Assert.Equal("Horário já registrado", msg.mensagem);

        }
    }
}
