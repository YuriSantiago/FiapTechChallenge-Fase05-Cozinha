using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IPedidoControleCozinhaRepository : IRepository<PedidoControleCozinha>
    {
        IList<PedidoControleCozinha> GetAllByStatus(StatusPedido statusPedido);
    }
}
