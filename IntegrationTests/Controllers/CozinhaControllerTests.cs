using Core.DTOs;
using Core.Entities;
using Core.Requests.Update;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests.Controllers
{
    public class CozinhaControllerTests : IClassFixture<CustomWebApplicationFactory<CozinhaProdutor.Program>>
    {
        private readonly HttpClient _client;

        public CozinhaControllerTests(CustomWebApplicationFactory<CozinhaProdutor.Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk()
        {
            // Act
            var response = await _client.GetAsync("/Cozinha");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var pedidosControleCozinha = await response.Content.ReadFromJsonAsync<List<PedidoControleCozinhaDTO>>();
            Assert.NotNull(pedidosControleCozinha);
            Assert.True(pedidosControleCozinha.Count >= 0);
        }

        [Fact]
        public async Task GetAllByStatus_ShouldReturnOk_WhenStatusExists()
        {
            // Arrange
            var status = StatusPedido.Pendente;

            // Act
            var response = await _client.GetAsync($"/Cozinha/{status}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var pedidosControleCozinha = await response.Content.ReadFromJsonAsync<List<PedidoControleCozinhaDTO>>();
            Assert.NotNull(pedidosControleCozinha);
            Assert.True(pedidosControleCozinha.Count >= 0);
        }

        [Fact]
        public async Task GetAllByStatus_ShouldReturnNotResults_WhenStatusDoesNotExist()
        {
            // Arrange
            string status = "Inexistente";

            // Act
            var response = await _client.GetAsync($"/Cozinha/{status}");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task AtualizarStatusPedido_ShouldReturnOK_WhenRequestIsValid()
        {
            // Arrange
            var pedidoUpdateStatusRequest = new PedidoUpdateStatusRequest
            {
                PedidoId = 1,
                Status = "Aceito"
            };

            // Act
            var response = await _client.PutAsJsonAsync("/Cozinha", pedidoUpdateStatusRequest);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task AtualizarStatusPedido_ShouldReturnBadRequest_WhenRequesIsInvalid()
        {
            // Arrange
            var pedidoUpdateStatusRequest = new PedidoUpdateStatusRequest
            {
                PedidoId = 1,
                Status = ""
            };

            // Act
            var response = await _client.PutAsJsonAsync("/Cozinha", pedidoUpdateStatusRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

    }
}
