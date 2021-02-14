namespace Genocs.CardVerificationMVC.Models
{
    public class MotoResultViewModel
    {
        public string Result { get; set; }

        public string AuthorizationCode { get; set; }

        public string PaymentId { get; set; }

        public string MerchantOrderId { get; set; }

        public string CustomField { get; set; }

        public string RRN { get; set; }

        public string ResponseCode { get; set; }

        public string Description { get; set; }

        public string CardCountry { get; set; }

        public string CardType { get; set; }
    }
}
