using Microsoft.VisualStudio.TestTools.UnitTesting;
using DesafioIlha.ControleDePonto.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesafioIlha.ControleDePonto.Models;
using DesafioIlha.ControleDePonto.Services.Interfaces;
using Telerik.JustMock;

namespace DesafioIlha.ControleDePonto.Controllers.Tests
{
    [TestClass()]
    public class BatidasControllerTests
    {
        [TestMethod()]
        public void BaterPontoTest()
        {
            Momento momento = new Momento()
            {
                Id = 1,
                dataHora = DateTime.Now,
            };

            var mockMomentoRepo = Mock.Create<IMomentoService>();
            Mock.Arrange(() => mockMomentoRepo.BaterPonto(Arg.IsAny<Momento>()))
                .Returns(() => momento);

            var momentoSalvo = mockMomentoRepo.BaterPonto(momento);

            Assert.IsNotNull(momentoSalvo);
            Assert.Equals(momento.Id, momentoSalvo.Id);
        }
    }
}