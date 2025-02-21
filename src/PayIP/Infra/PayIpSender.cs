using PayIP.Infra.Interfaces;
using PayIP.Model;
using PayIP.Model.PayIP.Model;
using System.Dynamic;
using System.Text;
using System.Text.Json;

namespace PayIP.Infra
{
    public class PayIpSender : IPayIpSender
    {
        private readonly ILogger<PayIpSender> _logger;
        private readonly HttpClient _httpClient;

        private readonly string _authUrl = "https://api.prod.payip.com.br/auth/realms/portal/protocol/openid-connect/client/token";
        private readonly string _loginUrl = "https://keycloak.hml.payip.com.br/realms/portal/protocol/openid-connect/token";

        public PayIpSender(ILogger<PayIpSender> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _httpClient = clientFactory.CreateClient();
        }

        public async Task<TokenResponse> GetAuthTokenAsync(string clientId, string clientSecret)
        {
            try
            {
                // Monta o corpo da requisição como x-www-form-urlencoded
                var formData = new StringContent(
                    $"grant_type=client_credentials&client_id={clientId}&client_secret={clientSecret}",
                    Encoding.UTF8,
                    "application/x-www-form-urlencoded"
                );

                // Envia a requisição POST
                var response = await _httpClient.PostAsync(_authUrl, formData);

                // Lê o conteúdo da resposta
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Token de autenticação obtido com sucesso.");


                    return JsonSerializer.Deserialize<TokenResponse>(responseContent); ;
                }
                else
                {
                    _logger.LogError($"Falha ao obter o token. StatusCode: {response.StatusCode}, Resposta: {responseContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao enviar requisição de autenticação: {ex.Message}");
                return null;
            }
        }

        public async Task<TokenResponse> GetLoginMotoraTokenAsync(string user, string pass)
        {
            try
            {
                // Monta o corpo da requisição como x-www-form-urlencoded
                var formData = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type", "password" },
                    { "client_id", "payip-auth-portal" },
                    { "username", user },
                    { "password", pass },
                });

                // Envia a requisição POST
                var response = await _httpClient.PostAsync(_loginUrl, formData);

                // Lê o conteúdo da resposta
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Token de autenticação obtido com sucesso.");


                    return JsonSerializer.Deserialize<TokenResponse>(responseContent); ;
                }
                else
                {
                    _logger.LogError($"Falha ao obter o token. StatusCode: {response.StatusCode}, Resposta: {responseContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao enviar requisição de autenticação: {ex.Message}");
                return null;
            }
        }

        public async Task<AllPaymentResponse> GetPagamentosByMotorista(string mapaId, string token, string? status, string? taxPayer, string? nf)
        {
            try
            {
                var url = $"https://api.hml.payip.com.br/v1/drivers/payments/list?driverRouter={mapaId}&paymentMethod=AVISTA&paymentShape=PIX";


                if (!string.IsNullOrEmpty(status))
                {
                    url += $"&status={status}";
                }

                if (!string.IsNullOrEmpty(taxPayer))
                {
                    url += $"&taxPayer={taxPayer}";
                }

                if (!string.IsNullOrEmpty(nf))
                {
                    url += $"&invoice={nf}";
                }

                var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
                httpRequest.Headers.Add("Authorization", $"Bearer {token}");

                // Envia a requisição
                var response = await _httpClient.SendAsync(httpRequest);

                // Lê o conteúdo da resposta
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Pagamentos pendentes obtidos com sucesso.");
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    // Converte o JSON da resposta para dynamic
                    return JsonSerializer.Deserialize<AllPaymentResponse>(responseContent, options);
                }
                else
                {
                    _logger.LogError($"Falha ao obter pagamentos. StatusCode: {response.StatusCode}, Resposta: {responseContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao enviar requisição para obter pagamentos: {ex.Message}");
                return null;
            }
        }

        public async Task<List<PaymentResponse>> GetPendingPaymentsAsync(string cpf, string token)
        {
            try
            {
                // Substituir {cpf} na URL pelo CPF fornecido
                var url = $"https://api.prod.payip.com.br/v1/payments/payer/{cpf}/pending";
                // Cria a requisição GET
                var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
                httpRequest.Headers.Add("Authorization", $"Bearer {token}");

                // Envia a requisição
                var response = await _httpClient.SendAsync(httpRequest);

                // Lê o conteúdo da resposta
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Pagamentos pendentes obtidos com sucesso.");
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    // Converte o JSON da resposta para dynamic
                    return JsonSerializer.Deserialize<List<PaymentResponse>>(responseContent, options);
                }
                else
                {
                    _logger.LogError($"Falha ao obter pagamentos pendentes. StatusCode: {response.StatusCode}, Resposta: {responseContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao enviar requisição para obter pagamentos pendentes: {ex.Message}");
                return null;
            }
        }

        public async Task<byte[]> GetPaymentStatementPdfAsync(string companyId, string token)
        {
            try
            {
                // Monta a URL com o companyId
                var url = $"https://api.prod.payip.com.br/v1/payments/statement/pdf?companyId={companyId}";

                // Cria a requisição GET
                var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
                httpRequest.Headers.Add("Authorization", $"Bearer {token}");

                // Envia a requisição
                var response = await _httpClient.SendAsync(httpRequest);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("PDF obtido com sucesso.");

                    // Retorna o conteúdo do PDF como array de bytes
                    return await response.Content.ReadAsByteArrayAsync();
                }
                else
                {
                    // Log de erro em caso de falha na requisição
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Falha ao obter o PDF. StatusCode: {response.StatusCode}, Resposta: {responseContent}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Log de erro em caso de exceção
                _logger.LogError($"Erro ao obter o PDF: {ex.Message}");
                return null;
            }
        }


    }
}
