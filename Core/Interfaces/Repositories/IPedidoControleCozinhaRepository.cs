using Core.Entities;
using Core.Enums;

namespace Core.Interfaces.Repositories
{
    public interface IPedidoControleCozinhaRepository : IRepository<PedidoControleCozinha>
    {
        IList<PedidoControleCozinha> GetAllByStatus(StatusPedido statusPedido);

        PedidoControleCozinha? GetByPedidoId(int pedidoId);

    }
}
