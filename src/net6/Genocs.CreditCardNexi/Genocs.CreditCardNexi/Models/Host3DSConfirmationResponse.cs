using System.Xml.Serialization;

namespace Genocs.CreditCardNexi.Models;

public class Host3DSConfirmationResponse
{
    public string authorizationcode { get; set; }
    public string cardcountry { get; set; }
    public string cardexpirydate { get; set; }
    public string cardtype { get; set; }
    public string customfield { get; set; }
    public string maskedpan { get; set; }
    public string merchantorderid { get; set; }
    public string paymentid { get; set; }
    public string responsecode { get; set; }
    public string result { get; set; }
    public string rrn { get; set; }
    public string securitytoken { get; set; }
    public string threedsecure { get; set; }
}
