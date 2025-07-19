using Core.Entities;
using Core.Interfaces.Services;
using Core.Requests.Update;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CozinhaProdutor.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class CozinhaController : ControllerBase
    {
        // Comentário para teste de esteira
        private readonly IBus _bus;
        private readonly IConfiguration _configuration;
        private readonly IPedidoControleCozinhaService _pedidoControleCozinhaService;


        public CozinhaController(IBus bus, IConfiguration configuration, IPedidoControleCozinhaService pedidoControleCozinhaService)
        {
            _bus = bus;
            _configuration = configuration;
            _pedidoControleCozinhaService = pedidoControleCozinhaService;
        }

        /// <summary>
        /// Busca todos os pedidos da cozinha cadastrados
        /// </summary>
        /// <returns>Retorna todos os pedidos da cozinha cadastrados</returns>
        /// <response code="200">Listagem retornada com sucesso</response>
        /// <response code="400">Erro ao listar os pedidos da cozinha</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpGet]
        [Authorize(Roles = "ADMIN, FUNCIONARIO")]
        public IActionResult Get()
        {
            try
            {
                return Ok(_pedidoControleCozinhaService.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Busca todos os pedidos da cozinha por status
        /// </summary>
        /// <param name="status">Status do pedido da cozinha</param>
        /// <returns>Retorna todos os pedidos da cozinha filtrados por status</returns>
        /// <response code="200">Listagem retornada com sucesso</response>
        /// <response code="400">Erro ao listar os pedidos da cozinha filtrados por status</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpGet("{status}")]
        [Authorize(Roles = "ADMIN, FUNCIONARIO")]
        public IActionResult Get([FromRoute] StatusPedido status)
        {
            try
            {
                return Ok(_pedidoControleCozinhaService.GetAllByStatus(status));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza o status do pedido na cozinha e para o cliente
        /// </summary>
        /// <param name="pedidoUpdateStatusRequest">Objeto do tipo "PedidoUpdateStatusRequest"</param>
        /// <response code="200">Status do pedido atualizado com sucesso</response>
        /// <response code="400">Erro ao atualizar o status do pedido</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPut]
        [Authorize(Roles = "ADMIN, FUNCIONARIO")]
        public async Task<IActionResult> AtualizarStatusPedido([FromBody] PedidoUpdateStatusRequest pedidoUpdateStatusRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var nomeFila = _configuration.GetSection("MassTransit:Queues")["PedidoAtualizacaoStatusQueue"] ?? string.Empty;
                var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{nomeFila}"));
                await endpoint.Send(pedidoUpdateStatusRequest);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }


    }
}
