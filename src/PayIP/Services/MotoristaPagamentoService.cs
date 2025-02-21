using PayIP.Infra.Interfaces;
using PayIP.Model;
using PayIP.Services.Interfaces;
using System.Globalization;
using System.Text;

namespace PayIP.Services
{
    public class MotoristaPagamentoService : IMotoristaPagamentoService
    {
        private readonly ILogger<MotoristaPagamentoService> _logger;
        private readonly IPayIpSender _payIpSender;

        public MotoristaPagamentoService(ILogger<MotoristaPagamentoService> logger, IPayIpSender payIpSender)
        {
            _logger = logger;
            _payIpSender = payIpSender;
        }

        public async Task<List<PagamentoInfosBot>> ObterPagamentosAsync(string mapaId, string motorista, string password, string? status, string? taxPayer, string? nf)
        {
            var token = await _payIpSender.GetLoginMotoraTokenAsync(motorista, password);

            var pagamentos = await _payIpSender.GetPagamentosByMotorista(mapaId, token.AccessToken, status, taxPayer, nf);

            var pagamentosBot = pagamentos.Data
                    .Where(pay => pay.QrCodePixCashin is not null)
                    .Select(pay => new PagamentoInfosBot
                    {
                        CodigoPix = pay.QrCodePixCashin.Emv,
                        DataDeVencimento = pay.DueDate.ToString("dd/MM/yyyy"),
                        Descricao = pay.Description,
                        Empresa = pay.Company.Name,
                        Id = pay.Id,
                        Valor = pay.Amount,
                        Cliente = pay.Client.Name ?? string.Empty,
                        CNPJ = pay.TaxPayerId ?? string.Empty,
                        Title = pay.Title ?? string.Empty,
                        ClienteCPF = pay.Client.TaxPayerId ?? string.Empty
                    })
                .ToList();

            return pagamentosBot;
        }

        public string GerarRelatorioPagamentosPendentes(List<PagamentoInfosBot> pagamentos)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Pagamentos pendentes");

            int contador = 1;
            foreach (var pagamento in pagamentos)
            {
                var valorFormatado = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", pagamento.Valor);

                sb.AppendLine($"{contador} - {pagamento.Empresa} - {valorFormatado} - {pagamento.DataDeVencimento}");
                pagamento.IdInterno = contador;
                contador++;
            }

            return sb.ToString();
        }

        public string GerarRelatorioPagamentosConcluidos(List<PagamentoInfosBot> pagamentos)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Pagamentos Concluidos");

            int contador = 1;
            foreach (var pagamento in pagamentos)
            {
                var valorFormatado = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", pagamento.Valor);

                sb.AppendLine($"{contador} - {pagamento.Empresa} - {valorFormatado} - {pagamento.DataDeVencimento}");
                pagamento.IdInterno = contador;
                contador++;
            }

            return sb.ToString();
        }
    }
}
