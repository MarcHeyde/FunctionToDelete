using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PersonService;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace FunctionToDelete
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestMessage req,
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
            PersonNumberRequest personNumberRequest = new PersonNumberRequest();
            PersonNumberCriteriaType criteriaType = new PersonNumberCriteriaType();
            criteriaType.PersonNumber = "rrn";
            personNumberRequest.Criteria = criteriaType;


           
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls12
            | SecurityProtocolType.Ssl3;
            string requestBody = await new StreamReader(await req.Content.ReadAsStreamAsync()).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
          
            var client = await CreateClientAsync();
           log.Warning((await client.PingAsync(new PingRequest())).PingResponse.PingResult);
            log.Warning("Succes");
          //  var result = await client.QueryOnNameAsync(new NameRequest() { Context = new RequestContextType() { UserContext = new UserContextType() }, Criteria = new NameCriteriaType() { Name = new NameType() { lastName = new string[] { "Charue" } } } });
            return new OkObjectResult(3);
        }

        private static async Task<FindPersonClient> CreateClientAsync()
        {//www.jsb-acc.wss.just.fgov.be
            EndpointIdentity identity = EndpointIdentity.CreateDnsIdentity("www.justid-acc.client.just.fgov.be");
            EndpointAddress endpointAddress = new
            EndpointAddress(new Uri("https://jsb-acc.just.fgov.be/2.00/CPS_FindPersonService"), identity);
            var client= new FindPersonClient(CreateClientBinding(), endpointAddress);

            var _keystore = new KeyStore();
            var clientCertificate = await _keystore.GetClientCertificate();
            client.ClientCredentials.ClientCertificate.Certificate = clientCertificate;             // Specify a default certificate for the service.
         //   var serviceCertificate = await _keystore.GetServiceCertificate();
            client.ClientCredentials.ServiceCertificate.DefaultCertificate = clientCertificate;
            client.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode =X509CertificateValidationMode.None;
            return client;
        }
        private static Binding CreateClientBinding()
        {
            var binding = new CustomBinding
            {
                OpenTimeout = TimeSpan.FromSeconds(20),
                CloseTimeout = TimeSpan.FromSeconds(20),
                SendTimeout = TimeSpan.FromSeconds(30),
                ReceiveTimeout = TimeSpan.FromSeconds(30),
            };             // 1. WS-Security
            // https://docs.microsoft.com/en-us/dotnet/api/system.servicemodel.channels.asymmetricsecuritybindingelement?view=netframework-4.7.2
            var asbe = SecurityBindingElement.CreateMutualCertificateDuplexBindingElement(
 MessageSecurityVersion
 .WSSecurity10WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10);
            asbe.DefaultAlgorithmSuite = SecurityAlgorithmSuite.Basic256Sha256;
            asbe.IncludeTimestamp = true;
            asbe.SetKeyDerivation(false);
            asbe.KeyEntropyMode = SecurityKeyEntropyMode.CombinedEntropy;
            asbe.SecurityHeaderLayout = SecurityHeaderLayout.Lax;
            asbe.ProtectTokens = false;
            asbe.EnableUnsecuredResponse = true;
            asbe.AllowSerializedSigningTokenOnReply = true;
            // 2. WS-Addressing - Soap12 disables addressing from the WS binding. Addressing elements are included in the service reference
            var msgEnc =
 new MtomMessageEncodingBindingElement(MessageVersion.Soap12, Encoding.UTF8);             // 3. transport HTTPS
            var https = new HttpsTransportBindingElement
            {
                KeepAliveEnabled = true,
                MaxReceivedMessageSize = 32 * 1048576, // 32MB
            }; binding.Elements.Add(asbe);
            binding.Elements.Add(msgEnc);
            binding.Elements.Add(https);
            return binding;
        }
    }
}
