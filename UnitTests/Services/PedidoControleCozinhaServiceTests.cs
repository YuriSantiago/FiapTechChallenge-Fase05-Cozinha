using Core.Entities;
using Core.Helpers;
using Core.Interfaces.Repositories;
using Core.Requests.Update;
using Core.Services;
using Moq;

namespace UnitTests.Services
{
    public class ContatoServiceTests
    {

        private readonly Mock<IPedidoControleCozinhaRepository> _pedidoControleCozinhaRepositoryMock;
        private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
        private readonly PedidoControleCozinhaService _pedidoControleCozinhaService;

        public ContatoServiceTests()
        {
            _pedidoControleCozinhaRepositoryMock = new Mock<IPedidoControleCozinhaRepository>();
            _pedidoRepositoryMock = new Mock<IPedidoRepository>();
            _pedidoControleCozinhaService = new PedidoControleCozinhaService(_pedidoControleCozinhaRepositoryMock.Object, _pedidoRepositoryMock.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnListOfPedidoControleCozinhaDTO()
        {
            // Arrange
            var pedidosControleCozinha = new List<PedidoControleCozinha>
            {
               new() {
                    Id = 1,
                    DataInclusao = DateTime.Now,
                    PedidoId = 1,
                    NomeCliente = "Yuri",
                    Status = StatusPedido.Pendente,
                    Pedido = new Pedido
                    { Id = 1,
                    DataInclusao = DateTime.Now,
                    UsuarioId = 1,
                    PrecoTotal = 50.00M,
                    Status = StatusPedido.Pendente,
                    TipoEntrega = "DELIVERY",
                    Usuario = new Usuario {Id = 1, Nome = "Yuri", Email = "yuri@email.com", Senha = Base64Helper.Encode("yuri"), Role = "ADMIN"}}
               }
             };

            _pedidoControleCozinhaRepositoryMock.Setup(r => r.GetAll(It.IsAny<Func<IQueryable<PedidoControleCozinha>, IQueryable<PedidoControleCozinha>>>())).Returns(pedidosControleCozinha);

            // Act
            var result = _pedidoControleCozinhaService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pedidosControleCozinha.Count, result.Count);
        }

        [Fact]
        public void GetAllByStatus_ShouldReturnListOfPedidoControleCozinhaDTOByStatus()
        {
            // Arrange
            var pedidosControleCozinha = new List<PedidoControleCozinha>
            {
               new() {
                    Id = 1,
                    DataInclusao = DateTime.Now,
                    PedidoId = 1,
                    NomeCliente = "Yuri",
                    Status = StatusPedido.Pendente,
                    Pedido = new Pedido
                    { Id = 1,
                    DataInclusao = DateTime.Now,
                    UsuarioId = 1,
                    PrecoTotal = 50.00M,
                    Status = StatusPedido.Pendente,
                    TipoEntrega = "DELIVERY",
                    Usuario = new Usuario {Id = 1, Nome = "Yuri", Email = "yuri@email.com", Senha = Base64Helper.Encode("yuri"), Role = "ADMIN"}}
               }
             };

            _pedidoControleCozinhaRepositoryMock.Setup(repo => repo.GetAllByStatus(StatusPedido.Pendente)).Returns(pedidosControleCozinha);

            // Act
            var result = _pedidoControleCozinhaService.GetAllByStatus(StatusPedido.Pendente);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pedidosControleCozinha.Count, result.Count);
        }

        [Fact]
        public void AtualizarStatusPedido_ShouldUpdatePedidoAndControleCozinha_WhenEntitiesExist()
        {
            // Arrange
            var request = new PedidoUpdateStatusRequest
            {
                PedidoId = 1,
                Status = "Pronto"
            };

            var pedido = new Pedido
            {
                Id = 1,
                DataInclusao = DateTime.Now,
                UsuarioId = 1,
                PrecoTotal = 50.00M,
                Status = StatusPedido.Pendente,
                TipoEntrega = "DELIVERY",
                Usuario = new Usuario
                {
                    Id = 1,
                    Nome = "Yuri",
                    Email = "yuri@email.com",
                    Senha = Base64Helper.Encode("yuri"),
                    Role = "ADMIN"
                }
            };

            var controle = new PedidoControleCozinha
            {
                Id = 1,
                DataInclusao = DateTime.Now,
                PedidoId = 1,
                NomeCliente = "Yuri",
                Status = StatusPedido.Pendente,
                Pedido = pedido
            };

            _pedidoRepositoryMock.Setup(r => r.GetById(1)).Returns(pedido);
            _pedidoControleCozinhaRepositoryMock.Setup(r => r.GetByPedidoId(1)).Returns(controle);

            // Act
            _pedidoControleCozinhaService.AtualizarStatusPedido(request);

            // Assert
            _pedidoRepositoryMock.Verify(r => r.Update(It.Is<Pedido>(p => p.Status == StatusPedido.Pronto)), Times.Once);
            _pedidoControleCozinhaRepositoryMock.Verify(r => r.Update(It.Is<PedidoControleCozinha>(c => c.Status == StatusPedido.Pronto)), Times.Once);
        }


    }
}
