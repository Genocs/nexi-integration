namespace Genocs.CreditCardNexi.Models;

public class Verify3DSViewModel
{
    public string Card3DSecureProviderURL { get; set; }
    public string MerchantId { get; set; }
    public string OrderRef { get; set; }
    public string CurrCode { get; set; }
    public string PaymetMethod { get; set; }
    public string CardNumber { get; set; }
    public string SecurityCode { get; set; }

    public string CardHolder { get; set; }
    public string ExpiredMonth { get; set; }
    public string ExpiredYear { get; set; }
    public string PayType { get; set; }
    public string SuccessUrl { get; set; }
    public string FailUrl { get; set; }
    public string ErrorUrl { get; set; }
    public string Lang { get; set; }
}
