namespace PayIP.Model
{
    public class GetPaymentModel
    {
        public string Motorista { get; set; }
        public string? Status { get; set; }
        public string Password { get; set; }
        public string MapaId { get; set; }
        public string? TaxPayerId { get; set; }
        public string? NF { get; set; }
    }
}
