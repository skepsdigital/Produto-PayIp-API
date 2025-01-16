using Microsoft.AspNetCore.Mvc;
using PayIP.Model;
using PayIP.Services.Interfaces;

namespace PayIP.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly IClientePagamentoService _clientePagamentoService;

        public ClienteController(ILogger<ClienteController> logger, IClientePagamentoService clientePagamentoService)
        {
            _logger = logger;
            _clientePagamentoService = clientePagamentoService;
        }

        [HttpGet("pagamentos/{cpf}")]
        public async Task<IActionResult> Add([FromRoute] string cpf)
        {
            _logger.LogInformation($"Iniciando processo de pegar pagamentos");

            var result = await _clientePagamentoService.ObterPagamentosPendentesAsync(cpf);
            var relatorio = _clientePagamentoService.GerarRelatorioPagamentosPendentes(result);

            if (result is not null)
            {
                return Ok(new {Relatorio = relatorio, Pagamentos = result});
            }

            return BadRequest();
        }
    }
}
