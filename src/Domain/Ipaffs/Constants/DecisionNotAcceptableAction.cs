namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class DecisionNotAcceptableAction
{
    public const string Slaughter = "slaughter";
    public const string Reexport = "reexport";
    public const string Euthanasia = "euthanasia";
    public const string Redispatching = "redispatching";
    public const string Destruction = "destruction";
    public const string Transformation = "transformation";
    public const string Other = "other";
    public const string EntryRefusal = "entry-refusal";
    public const string QuarantineImposed = "quarantine-imposed";
    public const string SpecialTreatment = "special-treatment";
    public const string IndustrialProcessing = "industrial-processing";
    public const string ReDispatch = "re-dispatch";
    public const string UseForOtherPurposes = "use-for-other-purposes";

    public static bool IsSlaughter(string? status) => Equals(Slaughter, status);

    public static bool IsReexport(string? status) => Equals(Reexport, status);

    public static bool IsEuthanasia(string? status) => Equals(Euthanasia, status);

    public static bool IsRedispatching(string? status) => Equals(Redispatching, status);

    public static bool IsDestruction(string? status) => Equals(Destruction, status);

    public static bool IsTransformation(string? status) => Equals(Transformation, status);

    public static bool IsOther(string? status) => Equals(Other, status);

    public static bool IsEntryRefusal(string? status) => Equals(EntryRefusal, status);

    public static bool IsQuarantineImposed(string? status) => Equals(QuarantineImposed, status);

    public static bool IsSpecialTreatment(string? status) => Equals(SpecialTreatment, status);

    public static bool IsIndustrialProcessing(string? status) => Equals(IndustrialProcessing, status);

    public static bool IsReDispatch(string? status) => Equals(ReDispatch, status);

    public static bool IsUseForOtherPurposes(string? status) => Equals(UseForOtherPurposes, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}
