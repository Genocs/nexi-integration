using System.Xml.Serialization;

namespace Genocs.CreditCardNexi.Models;

[XmlRoot(ElementName = "response")]
public class MotoResponseXmlResponse
{

    [XmlElement(ElementName = "result")]
    public string? Result { get; set; }

    [XmlElement(ElementName = "authorizationcode")]
    public string? Authorizationcode { get; set; }

    [XmlElement(ElementName = "paymentid")]
    public string? PaymentId { get; set; }

    [XmlElement(ElementName = "merchantorderid")]
    public string? MerchantorderId { get; set; }

    [XmlElement(ElementName = "rrn")]
    public double Rrn { get; set; }

    [XmlElement(ElementName = "responsecode")]
    public int ResponseCode { get; set; }

    [XmlElement(ElementName = "description")]
    public string? Description { get; set; }

    [XmlElement(ElementName = "cardcountry")]
    public string? CardCountry { get; set; }

    [XmlElement(ElementName = "cardtype")]
    public string? CardType { get; set; }
}

