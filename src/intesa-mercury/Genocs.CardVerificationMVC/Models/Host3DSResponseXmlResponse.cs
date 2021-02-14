
using System.Xml.Serialization;

namespace Genocs.CardVerificationMVC.Models
{
    [XmlRoot("response")]
    public class Host3DSResponseXmlResponse
    {
        [XmlElement(ElementName = "paymentid")]
        public string PaymentId { get; set; }

        [XmlElement(ElementName = "hostedpageurl")]
        public string HostedPageUrl { get; set; }

        [XmlElement(ElementName = "securitytoken")]
        public string SecurityToken { get; set; }
    }
}