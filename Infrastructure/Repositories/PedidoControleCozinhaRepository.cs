using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class PedidoControleCozinhaRepository : BaseRepository<PedidoControleCozinha>, IPedidoControleCozinhaRepository
    {

        public PedidoControleCozinhaRepository(ApplicationDbContext context) : base(context)
        {

        }

        public IList<PedidoControleCozinha> GetAllByStatus(StatusPedido statusPedido)
        {
            return [.. _context.PedidosControleCozinha.Where(r => r.Status == statusPedido)];
        }


    }
}
