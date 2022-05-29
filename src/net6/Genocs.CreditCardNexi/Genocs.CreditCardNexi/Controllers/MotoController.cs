using Genocs.CreditCardNexi.Extensions;
using Genocs.CreditCardNexi.Models;
using Genocs.CreditCardNexi.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Xml.Serialization;

namespace Genocs.CreditCardNexi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MotoController : ControllerBase
{
    private readonly TerminalOptions _options;

    public MotoController(IOptions<TerminalOptions> options)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    [HttpPost("RequestPayment")]
    public async Task<IActionResult> PostRequestPayment([FromBody] PaymentRequest request)
    {
        if (request == null)
        {
            return BadRequest("request cannot be null or empty");
        }

        if (string.IsNullOrWhiteSpace(request.OrderId))
        {
            return BadRequest("OrderId cannot be null empty");
        }

        if (request.Amount <= 0.0)
        {
            return BadRequest("Amount must be greather than zero");
        }

        if (string.IsNullOrWhiteSpace(request.CardHolderName))
        {
            return BadRequest("CardHolderName cannot be null empty");
        }

        if (string.IsNullOrWhiteSpace(request.PanToken))
        {
            return BadRequest("PanToken cannot be null empty");
        }

        if (!request.CVV2.IsCVV2Valid())
        {
            return BadRequest("CVV2 is invalid");
        }

        string url = $"{_options.BankUrl}?id={_options.Id}&password={_options.Password}&operationType={request.Operationtype}&amount={request.Amount:f2}&currencyCode={_options.CurrencyCode}&MerchantOrderId={request.OrderId}&description={request.OrderDescription}&cardHolderName={request.CardHolderName}&card={request.PanToken}&cvv2={request.CVV2}&expiryMonth={request.ExpiryMonth}&expiryYear={request.ExpiryYear}";
        MotoResponseXmlResponse? deserializedObject = null;
        using (HttpClient client = new())
        {
            using HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            using HttpResponseMessage response = await client.SendAsync(requestMessage);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(MotoResponseXmlResponse));

                using TextReader reader = new StreamReader(await response.Content.ReadAsStreamAsync());
                deserializedObject = deserializer.Deserialize(reader) as MotoResponseXmlResponse;
                reader.Close();
            }
        }

        return Ok(deserializedObject);
    }

    public class PaymentRequest
    {
        /// <summary>
        /// The Operation type [pay]
        /// </summary>
        public string Operationtype { get; set; } = "pay";

        public string? OrderId { get; set; }
        public string? OrderDescription { get; set; }
        public string? CardHolderName { get; set; }

        public string? PanToken { get; set; }

        public string? CVV2 { get; set; }
        public string? ExpiryMonth { get; set; }
        public string? ExpiryYear { get; set; }

        public double Amount { get; set; }
    }
}

