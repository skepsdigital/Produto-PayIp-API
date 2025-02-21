
using PayIP.Model;
using PayIP.Model.PayIP.Model;

namespace PayIP.Infra.Interfaces
{
    public interface IPayIpSender
    {
        Task<TokenResponse> GetAuthTokenAsync(string clientId, string clientSecret);
        Task<TokenResponse> GetLoginMotoraTokenAsync(string user, string pass);
        Task<AllPaymentResponse> GetPagamentosByMotorista(string mapaId, string token, string? status, string? taxPayer, string? nf);
        Task<byte[]> GetPaymentStatementPdfAsync(string companyId, string token);
        Task<List<PaymentResponse>> GetPendingPaymentsAsync(string cpf, string token);
    }
}
