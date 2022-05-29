
using System.Xml.Serialization;

namespace Genocs.CardVerificationMVC.Models
{
    [XmlRoot("response")]
    public class MotoResponseXmlResponse
    {
        [XmlElement(ElementName = "result")]
        public string Result { get; set; }

        [XmlElement(ElementName = "authorizationcode")]
        public string AuthorizationCode { get; set; }

        [XmlElement(ElementName = "paymentid")]
        public string PaymentId { get; set; }

        [XmlElement(ElementName = "merchantorderid")]
        public string MerchantOrderId { get; set; }


        [XmlElement(ElementName = "customfield")]
        public string CustomField { get; set; }


        [XmlElement(ElementName = "rrn")]
        public string RRN { get; set; }


        [XmlElement(ElementName = "responsecode")]
        public string ResponseCode { get; set; }

        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        [XmlElement(ElementName = "cardcountry")]
        public string CardCountry { get; set; }

        [XmlElement(ElementName = "cardtype")]
        public string CardType { get; set; }
    }
}