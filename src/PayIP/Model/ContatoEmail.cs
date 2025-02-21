namespace PayIP.Model
{
    public class ContatoEmail
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string Responsible { get; set; }
        public string Sector { get; set; }
        public string FeatureFlag { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public int IdInterno { get; set; }
    }
}
