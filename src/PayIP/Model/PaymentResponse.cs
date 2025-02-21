namespace PayIP.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    namespace PayIP.Model
    {
        public class Company
        {
            public string CompanyId { get; set; }
            public string TaxIdentifier { get; set; }
            public string Name { get; set; }
        }

        public class Client
        {
            public string ClientId { get; set; }
            public string TaxPayerId { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
        }

        public class AmountDetails
        {
            public decimal Amount { get; set; }
            public decimal AmountTotal { get; set; }
            public decimal AmountFine { get; set; }
            public decimal AmountInterest { get; set; }
        }

        public class AmountInterest
        {
            public bool HasInterest { get; set; }
            public decimal? AmountPerc { get; set; }
            public string Modality { get; set; }
        }

        public class AmountFine
        {
            public bool HasFine { get; set; }
            public decimal? AmountPerc { get; set; }
            public string Modality { get; set; }
        }

        public class QrCodePixCashin
        {
            public string Id { get; set; }
            public string LocationId { get; set; }
            public decimal Amount { get; set; }
            public string Status { get; set; }
            public string StatusPayment { get; set; }
            public DateTime Duedate { get; set; }
            public string Url { get; set; }
            public string Emv { get; set; }
            public string Type { get; set; }
            public int CalendarExpirationAfterPayment { get; set; }
            public DateTime CalendarCreatedAt { get; set; }
            public DateTime CalendarDueDate { get; set; }
        }

        public class Payment
        {
            public string Id { get; set; }
            public string ExternalId { get; set; }
            public Company Company { get; set; }
            public Client Client { get; set; }
            public string TaxPayerId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Status { get; set; }
            public DateTime? PaidDate { get; set; }
            public decimal? AmountPaid { get; set; }
            public decimal Amount { get; set; }
            public AmountDetails AmountDetails { get; set; }
            public DateTime DueDate { get; set; }
            public bool IsAutomaticDebit { get; set; }
            public bool IsPixCashIn { get; set; }
            public string HowWasitPaid { get; set; }
            public AmountInterest AmountInterest { get; set; }
            public AmountFine AmountFine { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
            public DateTime? DeletedAt { get; set; }
            public QrCodePixCashin QrCodePixCashin { get; set; }
        }

        public class AllPaymentResponse
        {
            [JsonPropertyName("page")]
            public int Page { get; set; }
            public int PagaSize { get; set; }
            public int Total { get; set; }
            public List<Payment> Data { get; set; }
        }

        public class PaymentResponse
        {
            [JsonPropertyName("companyId")]
            public string CompanyId { get; set; }
            public string CompanyName { get; set; }
            public List<Payment> Payments { get; set; }
        }
    }

}
