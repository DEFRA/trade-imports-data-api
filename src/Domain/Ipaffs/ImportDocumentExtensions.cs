using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;
using Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

public static class ImportDocumentExtensions
{
    public static string? GetChedType(this ImportDocument document)
    {
        return document.DocumentCode switch
        {
            "9115" or "C633" or "N002" or "N851" or "C085" => NotificationType.Chedpp,
            "N852" or "C678" => NotificationType.Ced,
            "C640" => NotificationType.Cveda,
            "C641" or "C673" or "N853" => NotificationType.Cvedp,
            _ => null,
        };
    }
}
