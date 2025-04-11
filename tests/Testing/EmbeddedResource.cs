namespace Defra.TradeImportsDataApi.Testing;

public static class EmbeddedResource
{
    public static string GetBody(Type anchor, string fileName)
    {
        using var stream = anchor.Assembly.GetManifestResourceStream($"{anchor.Namespace}.{fileName}");

        if (stream is null)
            throw new InvalidOperationException($"Unable to find embedded resource {fileName} in {anchor.Namespace}");

        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }
}
