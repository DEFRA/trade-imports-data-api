namespace Defra.TradeImportsDataApi.Testing;

public static class Endpoints
{
    public static class Gmrs
    {
        private const string Root = "/gmrs";

        public static string Get(string gmrId) => $"{Root}/{gmrId}";
    }

    public static class ImportNotifications
    {
        private const string Root = "/import-notifications";

        public static string Get(string chedId) => $"{Root}/{chedId}";
    }

    public static class CustomsDeclarations
    {
        private const string Root = "/customs-declarations";

        public static string Get(string mrn) => $"{Root}/{mrn}";
    }
}
