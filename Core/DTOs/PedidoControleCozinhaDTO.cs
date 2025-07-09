using System.Text.Json.Serialization;

namespace Core.DTOs
{
    public class PedidoControleCozinhaDTO
    {

        public required int Id { get; set; }

        public required int PedidoId {get; set;}

        public DateTime DataInclusao { get; set; }

        public required string NomeCliente { get; set; }

        public required string Status { get; set; }

        [JsonIgnore]
        public PedidoDTO? Pedido { get; set; }

        public ICollection<PedidoItemControleCozinhaDTO> Itens { get; set; } = [];


    }
}
