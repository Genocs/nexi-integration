namespace Genocs.CreditCardNexi.Settings;

public class TerminalOptions
{
    public static string Position = "Terminal";

    /// <summary>
    /// The terminal Id
    /// </summary>
    public string Id { get; set; } = default!;

    /// <summary>
    /// The terminal Password
    /// </summary>
    public string Password { get; set; } = default!;

    /// <summary>
    /// Root bank url
    /// </summary>
    public string BankUrl { get; set; } = default!;

    /// <summary>
    /// Root application url, used for 3DS 
    /// </summary>
    public string RootApplication { get; set; } = default!;


    /// <summary>
    /// This is the ISO 4217 currency code 
    /// </summary>
    public string CurrencyCode { get; set; } = default!;

}
