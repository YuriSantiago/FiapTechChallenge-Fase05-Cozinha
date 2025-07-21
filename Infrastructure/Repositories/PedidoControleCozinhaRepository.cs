using Core.Entities;
using Core.Enums;
using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PedidoControleCozinhaRepository : BaseRepository<PedidoControleCozinha>, IPedidoControleCozinhaRepository
    {

        public PedidoControleCozinhaRepository(ApplicationDbContext context) : base(context)
        {

        }

        public IList<PedidoControleCozinha> GetAllByStatus(StatusPedido statusPedido)
        {
            return [.. _context.PedidosControleCozinha.Where(r => r.Status == statusPedido).Include(pcc => pcc.Pedido).ThenInclude(p => p.Itens).ThenInclude(pi => pi.Produto)];
        }

        public PedidoControleCozinha? GetByPedidoId(int pedidoId)
        {
            return _context.PedidosControleCozinha.Where(r => r.PedidoId == pedidoId).FirstOrDefault();
        }


    }
}
