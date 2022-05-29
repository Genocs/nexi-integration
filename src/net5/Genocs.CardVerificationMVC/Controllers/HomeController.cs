using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Genocs.CardVerificationMVC.Controllers
{
    using Models;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _terminalId;
        private readonly string _terminalPassword;
        private readonly string _rootApplication;

        private readonly string _rootURL = "https://test.monetaonline.it/monetaweb/payment/2/xml";

        private readonly string Amount = "1.00";
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

        [HttpPost]
        public IActionResult CardVerificationPass(string authorizationcode, string cardcountry, string cardexpirydate,
            string cardtype, string customfield, string maskedpan,
            string merchantorderid, string paymentid, string responsecode,
            string result, string rrn, string securitytoken, string threedsecure)
        {


            return Ok("");
        }

        public IActionResult CardVerificationFail()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> VerifyHost3DS()
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_rootURL}?id={_terminalId}&password={_terminalPassword}&operationType=initialize&amount={Amount}&currencyCode={CurrencyCode}&language=ITA&responseToMerchantUrl={_rootApplication}Home/CardVerificationPass&recoveryUrl={_rootApplication}Home/CardVerificationFail&merchantOrderId=TRCK0001&description=Descrizione&cardHolderName=GiovanniNocco&cardHolderEmail=giovanni.nocco@gmail.com");
            HttpResponseMessage response = await client.SendAsync(requestMessage);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(Host3DSResponseXmlResponse));

                string s = await response.Content.ReadAsStringAsync();

                using (XmlTextReader reader = new XmlTextReader(new StringReader(await response.Content.ReadAsStringAsync())))
                {
                    Host3DSResponseXmlResponse deserializedObject = deserializer.Deserialize(reader) as Host3DSResponseXmlResponse;
                    reader.Close();
                    Response.Redirect($"{deserializedObject.HostedPageUrl}?paymentid={deserializedObject.PaymentId}", false);
                    return View(new DetailsViewModel
                    {
                        PaymentId = deserializedObject.PaymentId,
                        HostedPageUrl = deserializedObject.HostedPageUrl,
                        SecurityToken = deserializedObject.SecurityToken
                    });
                }
            }

            return Ok($"Response from Virtual pos is NOT OK. Result: '{response.StatusCode}'");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Moto()
        {
            string url = $"{_rootURL}?id={_terminalId}&password={_terminalPassword}&operationType=pay&amount={Amount}&currencyCode={CurrencyCode}&MerchantOrderId={MerchantOrderId}&description=Descrizione&cardHolderName=NomeCognome&card=4349942499990906&cvv2=034&expiryMonth=04&expiryYear=2023";
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
                        CustomField = deserializedObject.CustomField,
                        RRN = deserializedObject.RRN,
                        ResponseCode = deserializedObject.ResponseCode,
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
}
