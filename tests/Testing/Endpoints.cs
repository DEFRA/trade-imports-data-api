namespace Defra.TradeImportsDataApi.Testing;

public static class Endpoints
{
    public static class Gmrs
    {
        private const string Root = "/gmrs";

        public static string Get(string gmrId) => $"{Root}/{gmrId}";

        public static string Put(string gmrId) => $"{Root}/{gmrId}";
    }

    public static class ImportPreNotifications
    {
        private const string Root = "/import-pre-notifications";

        public static string Get(string chedId) => $"{Root}/{chedId}";

        public static string Put(string chedId) => $"{Root}/{chedId}";
    }

    public static class CustomsDeclarations
    {
        private const string Root = "/customs-declarations";

        public static string Get(string mrn) => $"{Root}/{mrn}";

        public static string Put(string mrn) => $"{Root}/{mrn}";
    }
}
