namespace PayIP.Model
{
    public class PagamentoInfosBot
    {
        public string Empresa { get; set; }
        public string Id { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public string DataDeVencimento { get; set; }
        public string CodigoPix { get; set; }

        public string Cliente { get; set; }
        public string ClienteCPF { get; set; }
        public string Title { get; set; }
        public string CNPJ { get; set; }

        public int IdInterno { get; set; }
    }
}
