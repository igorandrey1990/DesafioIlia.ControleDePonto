using Microsoft.VisualStudio.TestTools.UnitTesting;
using DesafioIlha.ControleDePonto.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DesafioIlha.ControleDePonto.Models;
using System.Reflection.Metadata;
using DesafioIlha.ControleDePonto.DAL;
using DesafioIlha.ControleDePonto.Services;
using DesafioIlha.ControleDePonto.Services.Interfaces;
using Telerik.JustMock;

namespace DesafioIlha.ControleDePonto.Controllers.Tests
{
    [TestClass()]
    public class FolhasDePontoControllerTests
    {
        [TestMethod()]
        public void Get_All_Momentos_Should_ReturnList()
        {
            var mockMomentoRepo = Mock.Create<IMomentoService>();
            Mock.Arrange(() => mockMomentoRepo.GetMomentos())
              .Returns(new List<Momento>
              {
                  new Momento()
                  {
                      Id = 1,
                      dataHora = DateTime.Parse("2023-04-26 08:00:00.0000000")
                  },
                  new Momento()
                  {
                      Id = 2,
                      dataHora = DateTime.Parse("2023-04-26 12:00:00.0000000")
                  },
                  new Momento()
                  {
                      Id = 3,
                      dataHora = DateTime.Parse("2023-04-26 13:00:00.0000000")
                  },
                  new Momento()
                  {
                      Id = 4,
                      dataHora = DateTime.Parse("2023-04-26 18:00:00.0000000")
                  },
                  new Momento()
                  {
                      Id = 5,
                      dataHora = DateTime.Parse("2023-04-27 08:00:00.0000000")
                  },
                  new Momento()
                  {
                      Id = 6,
                      dataHora = DateTime.Parse("2023-04-27 12:00:00.0000000")
                  },
                  new Momento()
                  {
                      Id = 7,
                      dataHora = DateTime.Parse("2023-04-27 13:00:00.0000000")
                  },
                  new Momento()
                  {
                      Id = 8,
                      dataHora = DateTime.Parse("2023-04-27 18:00:00.0000000")
                  }
              });

            var momentos = mockMomentoRepo.GetMomentos();

            // Assert
            Assert.IsNotNull(momentos);
            Assert.Equals(2, momentos.Count);
        }

        // ToDo
        [TestMethod()]
        public void GerarRelatorio_Should_Return_True()
        {
            var mockMomentoRepo = Mock.Create<IFolhaDePontoService>();
            Mock.Arrange(() => mockMomentoRepo.GetRelatorio("2023/05"))
              .Returns(new Relatorio()
              {
              });

        }
    }
}