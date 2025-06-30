namespace Core.Requests.Update
{
    public class PedidoUpdateStatusRequest
    {
        public required int PedidoId { get; set; }

        public required string Status { get; set; }

    }
}
