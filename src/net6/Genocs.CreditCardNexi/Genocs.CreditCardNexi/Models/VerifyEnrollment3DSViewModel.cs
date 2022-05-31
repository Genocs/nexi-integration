namespace Genocs.CreditCardNexi.Models;

public class VerifyEnrollment3DSViewModel
{
    /// <summary>
    /// The url where redirect the client
    /// </summary>
    public string? Card3DSecureProviderURL { get; set; }
    public string? PaReq { get; set; }
    public string? MD { get; set; }

    /// <summary>
    /// The return URL used to validate the response 
    /// </summary>
    public string? TermUrl { get; set; }
}
