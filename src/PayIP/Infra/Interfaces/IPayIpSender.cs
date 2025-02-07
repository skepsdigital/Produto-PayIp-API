
using PayIP.Model;
using PayIP.Model.PayIP.Model;

namespace PayIP.Infra.Interfaces
{
    public interface IPayIpSender
    {
        Task<TokenResponse> GetAuthTokenAsync(string clientId, string clientSecret);
        Task<byte[]> GetPaymentStatementPdfAsync(string companyId, string token);
        Task<List<PaymentResponse>> GetPendingPaymentsAsync(string cpf, string token);
    }
}
