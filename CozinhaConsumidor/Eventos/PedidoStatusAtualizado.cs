using Core.Interfaces.Services;
using Core.Requests.Update;
using MassTransit;

namespace PedidoConsumidor.Eventos
{
    public class PedidoStatusAtualizado : IConsumer<PedidoUpdateStatusRequest>
    {

        private readonly IPedidoControleCozinhaService _pedidoControleCozinhaService;

        public PedidoStatusAtualizado(IPedidoControleCozinhaService pedidoControleCozinhaService)
        {
            _pedidoControleCozinhaService = pedidoControleCozinhaService;
        }

        public Task Consume(ConsumeContext<PedidoUpdateStatusRequest> context)
        {
            _pedidoControleCozinhaService.AtualizarStatusPedido(context.Message);
            return Task.CompletedTask;
        }

    }
}
