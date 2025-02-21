using PayIP.Model;

namespace PayIP.Services.Interfaces
{
    public interface IMotoristaPagamentoService
    {
        string GerarRelatorioDeContatos(List<ContatoEmail> contatos);
        string GerarRelatorioPagamentosConcluidos(List<PagamentoInfosBot> pagamentos);
        string GerarRelatorioPagamentosPendentes(List<PagamentoInfosBot> pagamentos);
        Task<List<ContatoEmail>> GetContatosEmail(string motorista, string password);
        Task<List<PagamentoInfosBot>> ObterPagamentosAsync(string mapaId, string motorista, string password, string? status, string? taxPayer, string? nf);
    }
}
