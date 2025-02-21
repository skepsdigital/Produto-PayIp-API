using PayIP.Model;

namespace PayIP.Services.Interfaces
{
    public interface IMotoristaPagamentoService
    {
        string GerarRelatorioPagamentosConcluidos(List<PagamentoInfosBot> pagamentos);
        string GerarRelatorioPagamentosPendentes(List<PagamentoInfosBot> pagamentos);
        Task<List<PagamentoInfosBot>> ObterPagamentosAsync(string mapaId, string motorista, string password, string? status, string? taxPayer, string? nf);
    }
}
