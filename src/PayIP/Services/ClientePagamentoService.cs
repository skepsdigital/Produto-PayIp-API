using PayIP.Infra.Interfaces;
using PayIP.Model;
using PayIP.Services.Interfaces;
using System.Globalization;
using System.Text;

namespace PayIP.Services
{
    public class ClientePagamentoService : IClientePagamentoService
    {
        private readonly ILogger<ClientePagamentoService> _logger;
        private readonly IPayIpSender _payIpSender;

        private const string CLIENT_ID = "serv.hfbg645gdf.skelps";
        private const string CLIENT_SECRET = "jjAMdi5SK4oROVI6W1pWIUtlhL1UImJK";

        public ClientePagamentoService(IPayIpSender payIpSender, ILogger<ClientePagamentoService> logger)
        {
            _payIpSender = payIpSender;
            _logger = logger;
        }

        public async Task<byte[]> ObterRelatorioPDF(string companyId, string token) => await _payIpSender.GetPaymentStatementPdfAsync(companyId, token);

        public async Task<List<PagamentoInfosBot>> ObterPagamentosPendentesAsync(string cpf)
        {
            var token = await _payIpSender.GetAuthTokenAsync(CLIENT_ID, CLIENT_SECRET);

            var pagamentos = await _payIpSender.GetPendingPaymentsAsync(cpf, token.AccessToken);

            var pagamentosBot = pagamentos
                .SelectMany(p => p.Payments
                    .Where(pay => pay.QrCodePixCashin is not null)
                    .Select(pay => new PagamentoInfosBot
                    {
                        CodigoPix = pay.QrCodePixCashin.Emv,
                        DataDeVencimento = pay.DueDate.ToString("dd/MM/yyyy"),
                        Descricao = pay.Description,
                        Empresa = pay.Company.Name,
                        Id = pay.Id,
                        Valor = pay.Amount
                    }))
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
        
    }
}
