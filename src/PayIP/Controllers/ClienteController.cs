using Amazon.Lambda.APIGatewayEvents;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using PayIP.Model;
using PayIP.Services.Interfaces;
using System.Net.Mime;

namespace PayIP.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly IClientePagamentoService _clientePagamentoService;
        private readonly IMotoristaPagamentoService _motoristaPagamentoService;
        private readonly IAmazonS3 _s3client;
        private readonly string _bucketName;

        public ClienteController(ILogger<ClienteController> logger, IClientePagamentoService clientePagamentoService, IAmazonS3 s3client, IMotoristaPagamentoService motoristaPagamentoService)
        {
            _logger = logger;
            _clientePagamentoService = clientePagamentoService;
            _s3client = s3client;
            _bucketName = "produtos.com.br";
            _motoristaPagamentoService = motoristaPagamentoService;
        }

        [HttpGet("pagamentos/{cpf}")]
        public async Task<IActionResult> Add([FromRoute] string cpf)
        {
            _logger.LogInformation($"Iniciando processo de pegar pagamentos");

            var result = await _clientePagamentoService.ObterPagamentosPendentesAsync(cpf);
            var relatorio = _clientePagamentoService.GerarRelatorioPagamentosPendentes(result);

            if (result is not null && result.Any())
            {
                return Ok(new {Relatorio = relatorio, Pagamentos = result});
            }

            return BadRequest();
        }

        [HttpPost("contatos")]
        public async Task<IActionResult> ObterContatos([FromBody] GetPaymentModel getPaymentModel)
        {
            _logger.LogInformation($"Iniciando processo de pegar contatos");

            var result = await _motoristaPagamentoService.GetContatosEmail(getPaymentModel.Motorista, getPaymentModel.Password);

            var relatorio = string.Empty;

            relatorio = _motoristaPagamentoService.GerarRelatorioDeContatos(result);

            if (result is not null && result.Any())
            {
                return Ok(new { Relatorio = relatorio, Contatos = result });
            }

            return BadRequest();
        }

        [HttpPost("pagamentos")]
        public async Task<IActionResult> Add([FromBody] GetPaymentModel getPaymentModel)
        {
            _logger.LogInformation($"Iniciando processo de pegar pagamentos");

            var result = await _motoristaPagamentoService.ObterPagamentosAsync(getPaymentModel.MapaId, getPaymentModel.Motorista, getPaymentModel.Password, getPaymentModel.Status, getPaymentModel.TaxPayerId, getPaymentModel.NF);

            var relatorio = string.Empty;
            if(getPaymentModel.Status.Equals("PAID"))
            {
                relatorio = _motoristaPagamentoService.GerarRelatorioPagamentosConcluidos(result);
            }
            else
            {
                relatorio = _motoristaPagamentoService.GerarRelatorioPagamentosPendentes(result);
            }


            if (result is not null && result.Any())
            {
                return Ok(new { Relatorio = relatorio, Pagamentos = result });
            }

            return BadRequest();
        }

        [HttpGet("statement/pdf")]
        public async Task<IActionResult> GetPaymentStatementPdf([FromQuery] string companyId, [FromQuery] string token)
        {
            var result = await _clientePagamentoService.ObterRelatorioPDF(companyId, token);

            try
            {
                // Cria um MemoryStream a partir dos bytes do arquivo
                using var stream = new MemoryStream(result);

                // Configura a requisição de upload para o S3
                var putRequest = new PutObjectRequest
                {
                    BucketName = _bucketName, // Nome do bucket S3
                    Key = "relatorio"+Guid.NewGuid().ToString(),          // Nome do arquivo no S3
                    InputStream = stream,    // Conteúdo do arquivo
                    ContentType = "application/pdf", // Tipo MIME do arquivo
                };

                // Faz o upload para o S3
                await _s3client.PutObjectAsync(putRequest);

                // Gera a URL pré-assinada para o arquivo
                var url = GeneratePresignedUrl(putRequest.Key, TimeSpan.FromHours(24));
                return Ok(url);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao fazer upload para o S3: {ex.Message}");
                return BadRequest();
            }
        }

        private string GeneratePresignedUrl(string fileName, TimeSpan duration)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = fileName,
                Expires = DateTime.UtcNow.Add(duration),
            };

            return _s3client.GetPreSignedURL(request);
        }
    }
}
