namespace Defra.TradeImportsDataApi.Testing;

public static class Endpoints
{
    public static class RelatedImportDeclarations
    {
        private const string Root = "/related-import-declarations";

        public static string SearchByMrn(string mrn) => $"{Root}?mrn={mrn}";
    }

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

        public static string GetCustomsDeclarations(string chedId) => $"{Root}/{chedId}/customs-declarations";

        public static string GetGmrs(string chedId) => $"{Root}/{chedId}/gmrs";

        public static string Put(string chedId) => $"{Root}/{chedId}";

        public static string GetUpdates(EndpointQuery? query = null) => $"/import-pre-notification-updates{query}";
    }

    public static class CustomsDeclarations
    {
        private const string Root = "/customs-declarations";

        public static string Get(string mrn) => $"{Root}/{mrn}";

        public static string GetImportPreNotifications(string mrn) => $"{Root}/{mrn}/import-pre-notifications";

        public static string Put(string mrn) => $"{Root}/{mrn}";
    }

    public static class ProcessingErrors
    {
        private const string Root = "/processing-errors";

        public static string Get(string mrn) => $"{Root}/{mrn}";

        public static string Put(string mrn) => $"{Root}/{mrn}";
    }

    public static class Admin
    {
        public static string MaxId => "/admin/max-id";
    }
}
