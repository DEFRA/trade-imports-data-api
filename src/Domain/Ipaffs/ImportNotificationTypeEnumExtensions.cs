using Defra.TradeImportsDataApi.Domain.CustomsDeclaration;

namespace Defra.TradeImportsDataApi.Domain.Ipaffs;

public static class ImportNotificationTypeEnumExtensions
{
    public static ImportNotificationType? GetChedType(this string documentCode)
    {
        return documentCode switch
        {
            "9115" or "C633" or "N002" or "N851" or "C085" => ImportNotificationType.Chedpp,
            "N852" or "C678" => ImportNotificationType.Ced,
            "C640" => ImportNotificationType.Cveda,
            "C641" or "C673" or "N853" => ImportNotificationType.Cvedp,
            "9HCG" => null,
            _ => null,
        };
    }

    public static ImportNotificationType? GetChedType(this ImportDocument document)
    {
        return document?.DocumentCode?.GetChedType();
    }
}
