using System.Xml.Serialization;

namespace Genocs.CardVerificationMVC.Models
{
    [XmlRoot("error")]
    public class ErrorXmlResponse
    {
        [XmlElement(ElementName = "errorcode")]
        public string ErrorCode { get; set; }

        [XmlElement(ElementName = "errormessage")]
        public string ErrorMessage { get; set; }
    }
}

