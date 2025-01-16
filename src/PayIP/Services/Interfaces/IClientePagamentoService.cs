using PayIP.Model;

namespace PayIP.Services.Interfaces
{
    public interface IClientePagamentoService
    {
        string GerarRelatorioPagamentosPendentes(List<PagamentoInfosBot> pagamentos);
        Task<List<PagamentoInfosBot>> ObterPagamentosPendentesAsync(string cpf);
    }
}
