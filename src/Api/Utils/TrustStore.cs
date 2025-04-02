using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Defra.TradeImportsData.Api.Utils;

[ExcludeFromCodeCoverage]
public static class TrustStore
{
    public static void AddCustomTrustStore(this IServiceCollection _)
    {
        var certificates = GetCertificates();
        AddCertificates(certificates);
    }

    private static List<string> GetCertificates()
    {
        return Environment
            .GetEnvironmentVariables()
            .Cast<DictionaryEntry>()
            .Where(entry =>
                entry.Key.ToString()!.StartsWith("TRUSTSTORE") && IsBase64String(entry.Value!.ToString() ?? "")
            )
            .Select(entry =>
            {
                var data = Convert.FromBase64String(entry.Value!.ToString() ?? "");
                return Encoding.UTF8.GetString(data);
            })
            .ToList();
    }

    private static void AddCertificates(List<string> certificates)
    {
        if (certificates.Count == 0)
            return; // to stop trust store access denied issues on Macs
        var x509Certificate2S = certificates.Select(cert =>
            X509CertificateLoader.LoadCertificate(Encoding.ASCII.GetBytes(cert))
        );
        var certificateCollection = new X509Certificate2Collection();

        foreach (var certificate2 in x509Certificate2S)
        {
            certificateCollection.Add(certificate2);
        }

        var store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
        try
        {
            store.Open(OpenFlags.ReadWrite);
            store.AddRange(certificateCollection);
        }
        catch (Exception ex)
        {
            throw new FileLoadException("Root certificate import failed: " + ex.Message, ex);
        }
        finally
        {
            store.Close();
        }
    }

    private static bool IsBase64String(string str)
    {
        var buffer = new Span<byte>(new byte[str.Length]);
        return Convert.TryFromBase64String(str, buffer, out _);
    }
}
