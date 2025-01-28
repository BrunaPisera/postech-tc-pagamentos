using Microsoft.AspNetCore.Mvc;
using Pagamentos.Application.Exceptions;
using Pagamentos.Infrastructure.Models.MercadoPagoAPI;
using Pagamentos.UseCases.Dtos;
using Pagamentos.UseCases.Interfaces;

namespace Pagamentos.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PagamentoController : ControllerBase
    {
        private readonly IPagamentoUseCases PagamentoUseCases;

        public PagamentoController(IPagamentoUseCases pagamentoUseCases)
        {
            PagamentoUseCases = pagamentoUseCases;
        }

        [HttpPost("{idPagamento}/confirmarPagamento")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfirmarPagamento([FromRoute] Guid idPagamento, [FromBody] MPNotificacaoDePagamento notificacao)
        {
            if (idPagamento == Guid.Empty) return BadRequest("O id do pedido nao pode ser nulo.");

            if (string.IsNullOrEmpty(notificacao.topic) || notificacao.topic != "payment") return Ok();

            try
            {
                await PagamentoUseCases.ConfirmarPagamentoAsync(idPagamento);

                return Ok();
            }
            catch(PagamentoNaoEncontradoException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (ConfirmarPagamentoException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a requisição, tente novamente mais tarde.");
            }

        }

        [HttpPost("gerarPagamento")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GerarPagamento([FromBody] DadosPedidoDto dadosPedidoDto)
        {
            if (dadosPedidoDto == null) return BadRequest("O corpo da requisição não apresenta os dados do pedido.");

            if (string.IsNullOrEmpty(dadosPedidoDto.IdPedido)) return BadRequest("O id do pagamento não pode ser nulo.");

            try
            {
                var dadosQrCode = await PagamentoUseCases.GerarQrCodeParaPagamento(dadosPedidoDto);

                return Ok(dadosQrCode);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a requisição, tente novamente mais tarde.");
            }
        }
    }
}
