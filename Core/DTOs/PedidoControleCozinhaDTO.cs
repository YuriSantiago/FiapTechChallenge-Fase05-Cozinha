namespace Core.DTOs
{
    public class PedidoControleCozinhaDTO
    {

        public int Id { get; set; }

        public DateTime DataInclusao { get; set; }

        public required string NomeCliente { get; set; }

        public required string Status { get; set; }

        public PedidoDTO? Pedido { get; set; }

        public ICollection<PedidoItemControleCozinhaDTO> Itens { get; set; } = [];


    }
}
