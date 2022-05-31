using System.Xml.Serialization;

namespace Genocs.CreditCardNexi.Models;

[XmlRoot("response")]
public class EnrollmentResponseXmlResponse
{
    [XmlElement(ElementName = "result")]
    public string Result { get; set; }

    [XmlElement(ElementName = "paymentid")]
    public string PaymentId { get; set; }

    [XmlElement(ElementName = "customfield")]
    public string CustomField { get; set; }

    [XmlElement(ElementName = "description")]
    public string Description { get; set; }

    [XmlElement(ElementName = "merchantorderid")]
    public string MerchantOrderId { get; set; }

    /// <summary>
    /// Filled in case of ENROLLED
    /// </summary>
    [XmlElement(ElementName = "PAReq")]
    public string PAReq { get; set; }

    /// <summary>
    /// Filled in case of ENROLLED
    /// </summary>
    [XmlElement(ElementName = "url")]
    public string HostedPageUrl { get; set; }

    /// <summary>
    /// only in case of NOT Enrolled
    /// </summary>
    [XmlElement(ElementName = "eci")]
    public string ECI { get; set; }
}