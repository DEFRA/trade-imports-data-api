using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

public static class ImportDocumentExtensions
{
    public static string? GetChedType(this ImportDocument document)
    {
        return document.DocumentCode switch
        {
            "9115" or "C633" or "N002" or "N851" or "C085" => ImportNotificationType.Chedpp,
            "N852" or "C678" => ImportNotificationType.Ced,
            "C640" => ImportNotificationType.Cveda,
            "C641" or "C673" or "N853" => ImportNotificationType.Cvedp,
            _ => null,
        };
    }
}
