using System.Xml.Serialization;

namespace Genocs.CreditCardNexi.Models;

[XmlRoot("error")]
public class ErrorXmlResponse
{
    [XmlElement(ElementName = "errorcode")]
    public string ErrorCode { get; set; }

    [XmlElement(ElementName = "errormessage")]
    public string ErrorMessage { get; set; }
}

