using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Requests.Update;
using Microsoft.EntityFrameworkCore;

namespace Core.Services
{
    public class PedidoControleCozinhaService : IPedidoControleCozinhaService
    {
        private readonly IPedidoControleCozinhaRepository _pedidoControleCozinhaRepository;
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoControleCozinhaService(IPedidoControleCozinhaRepository pedidoControleCozinhaRepository, IPedidoRepository pedidoRepository)
        {
            _pedidoControleCozinhaRepository = pedidoControleCozinhaRepository;
            _pedidoRepository = pedidoRepository;
        }

        public IList<PedidoControleCozinhaDTO> GetAll()
        {
            var pedidosControleCozinha = _pedidoControleCozinhaRepository.GetAll(pcc => pcc.Include(pcc => pcc.Pedido)
                                                                                          .ThenInclude(pe => pe.Itens)
                                                                                          .ThenInclude(pi => pi.Produto));
            return ConvertToPedidoControleCozinhaDTO(pedidosControleCozinha);
        }

        public IList<PedidoControleCozinhaDTO> GetAllByStatus(StatusPedido statusPedido)
        {       
            var pedidosControleCozinha = _pedidoControleCozinhaRepository.GetAllByStatus(statusPedido);
            return ConvertToPedidoControleCozinhaDTO(pedidosControleCozinha);
        }

        public void AtualizarStatusPedido(PedidoUpdateStatusRequest pedidoUpdateStatusRequest)
        {
            var pedido = _pedidoRepository.GetById(pedidoUpdateStatusRequest.PedidoId);
            pedido.Status = (StatusPedido)Enum.Parse(typeof(StatusPedido), pedidoUpdateStatusRequest.Status);
            _pedidoRepository.Update(pedido);

            var pedidoControleCozinha = _pedidoControleCozinhaRepository.GetById(pedidoUpdateStatusRequest.PedidoId);
            pedidoControleCozinha.Status = (StatusPedido)Enum.Parse(typeof(StatusPedido), pedidoUpdateStatusRequest.Status);
            _pedidoControleCozinhaRepository.Update(pedidoControleCozinha);
        }

        private static List<PedidoControleCozinhaDTO> ConvertToPedidoControleCozinhaDTO(IList<PedidoControleCozinha> pedidosControleCozinha)
        {
            var pedidosControleCozinhaDTO = new List<PedidoControleCozinhaDTO>();
           
            foreach (var pedidoControleCozinha in pedidosControleCozinha)
            {
                var pedidosItemControleCozinhaDTO = new List<PedidoItemControleCozinhaDTO>();

                foreach (var item in pedidoControleCozinha.Pedido.Itens)
                {
                    pedidosItemControleCozinhaDTO.Add(new PedidoItemControleCozinhaDTO()
                    {
                        Nome = item.Produto.Nome,
                        Descricao = item.Produto.Descricao,
                        Quantidade = item.Quantidade
                    }); 
                }

                pedidosControleCozinhaDTO.Add(new PedidoControleCozinhaDTO()
                {
                    Id = pedidoControleCozinha.Id,
                    DataInclusao = pedidoControleCozinha.DataInclusao,
                    NomeCliente = pedidoControleCozinha.NomeCliente,
                    Status = pedidoControleCozinha.Status.ToString(),
                    Itens = pedidosItemControleCozinhaDTO
                });
            }

            return pedidosControleCozinhaDTO;

        }
    }

}
