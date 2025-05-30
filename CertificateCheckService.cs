using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

namespace YSCertificateCheck
{
    internal class CertificateCheckService
    {
        public static async Task<string> Check(string url)
        {
            var res = await Task.Run(() =>
            {
                string result = "No certificate found.";
                using (HttpClientHandler handler = new())
                {
                    handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                    {
                        if (cert != null)
                        {
                            X509Certificate2 certificate = new X509Certificate2(cert);
                            result = $"Issuer: {certificate.Issuer}";
                            result += $"\nSubject: {certificate.Subject}";
                            result += $"\nValid From: {certificate.NotBefore}";
                            result += $"\nValid Until: {certificate.NotAfter}";
                            result += $"\nThumbprint: {certificate.Thumbprint}";
                            result += $"\nSerial Number: {certificate.SerialNumber}";
                            result += $"\nSignature Algorithm: {certificate.SignatureAlgorithm.FriendlyName}";

                            return true; // Allow all certificates (for analysis)
                        }
                        else
                        {
                            return false;
                        }
                    };

                    using (HttpClient client = new(handler))
                    {
                        if (client != null)
                        {
                            var getResult = client.GetAsync(url).Result;
                            if (getResult.StatusCode != System.Net.HttpStatusCode.OK)
                            {
                                result = $"Status code: {getResult.StatusCode}";
                            }
                        }
                    }
                    return result;
                }
            });
            return res;
        }
    }
}
