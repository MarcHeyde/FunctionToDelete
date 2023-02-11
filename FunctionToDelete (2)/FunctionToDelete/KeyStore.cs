using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace FunctionToDelete
{
    public class KeyStore
    {
        private readonly CertificateClient _keyVaultClient;
        public KeyStore()
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            _keyVaultClient = new CertificateClient(new Uri("https://az-kv-justid-dev.vault.azure.net/"),new DefaultAzureCredential());
        }
        public async Task<X509Certificate2> GetClientCertificate()
        {
            var secret = await _keyVaultClient.DownloadCertificateAsync("https://az-kv-justid-dev.vault.azure.net/", "FindPersonCertificate");
            //var result = await _keyVaultClient.GetKeyAsync("https://az-kv-justid-dev.vault.azure.net/", "FindPersonCertificate");

            //var rsa = result.Key.ToRSA();
            //var tst = secret.Cer.ToList();

            //tst.AddRange(result.Key.N.ToList());
            
            return secret.Value;
        }
        public async Task<X509Certificate2> GetServiceCertificate()
        {

            ////var secret = await _keyVaultClient.GetCertificateAsync("https://az-kv-justid-dev.vault.azure.net/", "just-id2");
            ////var result = await _keyVaultClient.GetKeyAsync("https://az-kv-justid-dev.vault.azure.net/", "just-id2-key");
            ////var cert = new X509Certificate2(secret.Cer);
            //var rsa = result.Key.ToRSA();
            //rsa.ExportParameters(true);
            //cert.PrivateKey = rsa;
            //return cert;
            var secret = await _keyVaultClient.DownloadCertificateAsync("https://az-kv-justid-dev.vault.azure.net/", "FindPersonCertificate");
            //var result = await _keyVaultClient.GetKeyAsync("https://az-kv-justid-dev.vault.azure.net/", "FindPersonCertificate");

            //var rsa = result.Key.ToRSA();
            //var tst = secret.Cer.ToList();

            //tst.AddRange(result.Key.N.ToList());

            return secret.Value;
        }
    }
}
