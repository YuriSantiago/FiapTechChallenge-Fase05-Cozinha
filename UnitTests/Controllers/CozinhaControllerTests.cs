using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Requests.Update;
using CozinhaProdutor.Controllers;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace UnitTests.Controllers
{
    public class CozinhaControllerTests
    {

        private readonly Mock<IBus> _mockBus;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IPedidoControleCozinhaService> _mockPedidoControleCozinhaService;
        private readonly CozinhaController _cozinhaController;

        public CozinhaControllerTests()
        {
            _mockBus = new Mock<IBus>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockPedidoControleCozinhaService = new Mock<IPedidoControleCozinhaService>();
            _cozinhaController = new CozinhaController(_mockBus.Object, _mockConfiguration.Object, _mockPedidoControleCozinhaService.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnOkWithPedidosControleCozinha()
        {
            // Arrange
            var pedidosControleCozinha = new List<PedidoControleCozinhaDTO>
            {
               new() {
                    Id = 1,
                    PedidoId = 1,
                    DataInclusao = DateTime.Now,
                    NomeCliente = "Yuri",
                    Status = "Pendente"
               }
             };

            _mockPedidoControleCozinhaService.Setup(s => s.GetAll()).Returns(pedidosControleCozinha);

            // Act
            var result = _cozinhaController.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(pedidosControleCozinha, okResult.Value);
        }

        [Fact]
        public void GetAll_ShouldReturnBadRequest_WhenServiceThrowsException()
        {
            // Arrange
            _mockPedidoControleCozinhaService.Setup(s => s.GetAll()).Throws(new Exception("Erro inesperado"));

            // Act
            var result = _cozinhaController.Get();

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var value = badRequest.Value?.GetType().GetProperty("mensagem");
            Assert.NotNull(value);
            Assert.Equal("Erro inesperado", value.GetValue(badRequest.Value)?.ToString());
        }

        [Fact]
        public void GetAllByStatus_ShouldReturnOkWithPedidosControleCozinhaByStatus()
        {
            // Arrange
            var pedidosControleCozinha = new List<PedidoControleCozinhaDTO>
            {
               new() {
                    Id = 1,
                    PedidoId = 1,
                    DataInclusao = DateTime.Now,
                    NomeCliente = "Yuri",
                    Status = "Pendente"
               }
             };

            _mockPedidoControleCozinhaService.Setup(s => s.GetAllByStatus(StatusPedido.Pendente)).Returns(pedidosControleCozinha);

            // Act
            var result = _cozinhaController.Get(StatusPedido.Pendente);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, ok.StatusCode);
            Assert.Equal(pedidosControleCozinha, ok.Value);
        }

        [Fact]
        public void GetAllByCategory_ShouldReturnBadRequest_WhenServiceThrowsException()
        {
            // Arrange
            _mockPedidoControleCozinhaService.Setup(s => s.GetAllByStatus(It.IsAny<StatusPedido>())).Throws(new Exception("Erro ao buscar por status"));

            // Act
            var result = _cozinhaController.Get(StatusPedido.Pendente);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var value = badRequest.Value?.GetType().GetProperty("mensagem");
            Assert.NotNull(value);
            Assert.Equal("Erro ao buscar por status", value.GetValue(badRequest.Value)?.ToString());
        }

        [Fact]
        public async Task AtualizarStatusPedido_ShouldReturnOk_WhenRequestIsValid()
        {
            // Arrange
            var request = new PedidoUpdateStatusRequest
            {
                PedidoId = 1,
                Status = "Pronto"
            };

            var endpointMock = new Mock<ISendEndpoint>();
            _mockConfiguration.Setup(c => c.GetSection("MassTransit:Queues")["PedidoAtualizacaoStatusQueue"]).Returns("filaAtualizacaoStatusPedido");
            _mockBus.Setup(b => b.GetSendEndpoint(It.IsAny<Uri>())).ReturnsAsync(endpointMock.Object);

            // Act
            var result = await _cozinhaController.AtualizarStatusPedido(request);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            endpointMock.Verify(e => e.Send(request, default), Times.Once);
        }

        [Fact]
        public async Task AtualizarStatusPedido_ShouldReturnBadRequest_WhenQueueFails()
        {
            // Arrange
            var request = new PedidoUpdateStatusRequest
            {
                PedidoId = 1,
                Status = "Pronto"
            };

            _mockConfiguration.Setup(c => c.GetSection("MassTransit:Queues")["PedidoAtualizacaoStatusQueue"]).Returns("filaAtualizacaoStatusPedido");
            _mockBus.Setup(b => b.GetSendEndpoint(It.IsAny<Uri>())).ThrowsAsync(new Exception("Falha ao enviar mensagem"));

            // Act
            var result = await _cozinhaController.AtualizarStatusPedido(request);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var value = badRequest.Value?.GetType().GetProperty("mensagem")?.GetValue(badRequest.Value)?.ToString();
            Assert.Equal("Falha ao enviar mensagem", value);
        }
    }
}
