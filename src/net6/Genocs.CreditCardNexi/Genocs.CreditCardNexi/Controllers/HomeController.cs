using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

namespace Genocs.CreditCardNexi.Controllers;

using Genocs.CreditCardNexi.Models;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;
    private readonly string _terminalId;
    private readonly string _terminalPassword;
    private readonly string _rootApplication;

    private readonly string _rootURL = "https://test.monetaonline.it/monetaweb/payment/2/xml";

    private readonly string Amount = "11.50";
    private readonly string MerchantOrderId = "TRCK0001";
    private readonly string CurrencyCode = "978";

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _terminalId = _configuration.GetValue<string>("Terminal:Id");
        _terminalPassword = _configuration.GetValue<string>("Terminal:Password");
        _rootApplication = _configuration.GetValue<string>("Terminal:RootApplication");
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    //public IActionResult CardVerificationPass(Host3DSConfirmationResponse response)
    //{
    //    return View();
    //}

    //[HttpPost]

    public ActionResult CardVerificationPass(Verify3DSResponse response)
    {
        return View(new CardVerificationPassViewModel { MD = response.MD, PaRes = response.PaRes });
    }

    public IActionResult CardVerificationFail()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> VerifyEnrollment3DS()
    {
        List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();

        parameters.Add(new KeyValuePair<string, string>("id", _terminalId));
        parameters.Add(new KeyValuePair<string, string>("password", _terminalPassword));
        parameters.Add(new KeyValuePair<string, string>("operationType", "verifyenrollment"));

        parameters.Add(new KeyValuePair<string, string>("card", "4349942499990906"));
        parameters.Add(new KeyValuePair<string, string>("cvv2", "034"));

        parameters.Add(new KeyValuePair<string, string>("expiryyear", "2023"));
        parameters.Add(new KeyValuePair<string, string>("expirymonth", "12"));
        parameters.Add(new KeyValuePair<string, string>("cardholdername", "Nocco Giovanni"));
        parameters.Add(new KeyValuePair<string, string>("amount", "0.5"));

        parameters.Add(new KeyValuePair<string, string>("currencycode", CurrencyCode));
        parameters.Add(new KeyValuePair<string, string>("description", "description"));
        parameters.Add(new KeyValuePair<string, string>("customfield", "customdata"));
        parameters.Add(new KeyValuePair<string, string>("merchantorderid", MerchantOrderId));

        EnrollmentResponseXmlResponse? deserializedObject = null;

        using (HttpClient client = new())
        {
            using (HttpContent httpContent = new FormUrlEncodedContent(parameters))
            {
                HttpResponseMessage response = await client.PostAsync(_rootURL, httpContent);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(EnrollmentResponseXmlResponse));

                    using (XmlTextReader reader = new XmlTextReader(new StringReader(await response.Content.ReadAsStringAsync())))
                    {
                        deserializedObject = serializer.Deserialize(reader) as EnrollmentResponseXmlResponse;
                        reader.Close();
                    }
                }
            }
        }

        // Sent the result to the view to redirect to the verification page
        return View(new VerifyEnrollment3DSViewModel
        {
            Card3DSecureProviderURL = deserializedObject?.HostedPageUrl,
            MD = deserializedObject?.PaymentId,
            PaReq = deserializedObject?.PAReq,
            TermUrl = $"{_rootApplication}Home/CardVerificationPass"
        });
    }




    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> VerifyHost3DS()
    {
        List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();


        parameters.Add(new KeyValuePair<string, string>("id", _terminalId));
        parameters.Add(new KeyValuePair<string, string>("password", _terminalPassword));
        parameters.Add(new KeyValuePair<string, string>("operationType", "initialize"));

        parameters.Add(new KeyValuePair<string, string>("amount", Amount));
        parameters.Add(new KeyValuePair<string, string>("currencyCode", CurrencyCode));

        parameters.Add(new KeyValuePair<string, string>("language", "ITA"));
        parameters.Add(new KeyValuePair<string, string>("responseToMerchantUrl", $"{_rootApplication}Home/CardVerificationPass"));
        parameters.Add(new KeyValuePair<string, string>("recoveryUrl", $"{_rootApplication}Home/CardVerificationFail"));

        parameters.Add(new KeyValuePair<string, string>("merchantOrderId", "TRCK0001"));
        parameters.Add(new KeyValuePair<string, string>("description", "Descrizione"));

        parameters.Add(new KeyValuePair<string, string>("cardHolderName", "Nocco Giovanni"));
        parameters.Add(new KeyValuePair<string, string>("cardHolderEmail", "giovanni.nocco@gmail.com"));

        Host3DSResponseXmlResponse? deserializedObject = null;

        using (HttpClient client = new())
        {
            using (HttpContent httpContent = new FormUrlEncodedContent(parameters))
            {
                HttpResponseMessage response = await client.PostAsync(_rootURL, httpContent);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Host3DSResponseXmlResponse));

                    var tmp = await response.Content.ReadAsStringAsync();

                    using (XmlTextReader reader = new XmlTextReader(new StringReader(await response.Content.ReadAsStringAsync())))
                    {
                        deserializedObject = serializer.Deserialize(reader) as Host3DSResponseXmlResponse;
                        reader.Close();
                        if (deserializedObject != null)
                        {
                            Response.Redirect($"{deserializedObject.HostedPageUrl}?paymentid={deserializedObject.PaymentId}", false);
                            return View(new DetailsViewModel
                            {
                                PaymentId = deserializedObject.PaymentId,
                                HostedPageUrl = deserializedObject.HostedPageUrl,
                                SecurityToken = deserializedObject.SecurityToken
                            });
                        }
                    }
                }
            }
        }

        return Ok($"gg");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Moto()
    {
        string url = $"{_rootURL}?" +
            $"id={_terminalId}" +
            $"&password={_terminalPassword}" +
            $"&operationType=pay" +
            $"&amount={Amount}" +
            $"&currencyCode={CurrencyCode}" +
            $"&MerchantOrderId={MerchantOrderId}" +
            $"&description=Descrizione" +
            $"&cardHolderName=NomeCognome" +
            $"&card=4349942499990906" +
            $"&cvv2=034" +
            $"&expiryMonth=04" +
            $"&expiryYear=2023";

        HttpClient client = new HttpClient();
        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
        HttpResponseMessage response = await client.SendAsync(requestMessage);

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(MotoResponseXmlResponse));

            using (XmlTextReader reader = new XmlTextReader(new StringReader(await response.Content.ReadAsStringAsync())))
            {
                MotoResponseXmlResponse deserializedObject = deserializer.Deserialize(reader) as MotoResponseXmlResponse;
                reader.Close();

                return View(new MotoResultViewModel
                {
                    PaymentId = deserializedObject.PaymentId,
                    Result = deserializedObject.Result,
                    AuthorizationCode = deserializedObject.AuthorizationCode,
                    MerchantOrderId = deserializedObject.MerchantOrderId,
                    //CustomField = deserializedObject.CustomFiel,
                    RRN = deserializedObject.Rrn,
                    //                        ResponseCode = deserializedObject.ResponseCode,
                    Description = deserializedObject.Description,
                    CardCountry = deserializedObject.CardCountry,
                    CardType = deserializedObject.CardType
                });
            }
        }

        return Ok("");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
