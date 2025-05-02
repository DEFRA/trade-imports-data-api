namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class NotificationStatus
{
    public const string Draft = "DRAFT";
    public const string Submitted = "SUBMITTED";
    public const string Validated = "VALIDATED";
    public const string Rejected = "REJECTED";
    public const string InProgress = "IN_PROGRESS";
    public const string Amend = "AMEND";
    public const string Modify = "MODIFY";
    public const string Replaced = "REPLACED";
    public const string Cancelled = "CANCELLED";
    public const string Deleted = "DELETED";
    public const string PartiallyRejected = "PARTIALLY_REJECTED";
    public const string SplitConsignment = "SPLIT_CONSIGNMENT";
    public const string SubmittedInProgressModify = "SUBMITTED,IN_PROGRESS,MODIFY";

    public static bool IsDraft(string? status) => Equals(Draft, status);

    public static bool IsSubmitted(string? status) => Equals(Submitted, status);

    public static bool IsValidated(string? status) => Equals(Validated, status);

    public static bool IsRejected(string? status) => Equals(Rejected, status);

    public static bool IsInProgress(string? status) => Equals(InProgress, status);

    public static bool IsAmend(string? status) => Equals(Amend, status);

    public static bool IsModify(string? status) => Equals(Modify, status);

    public static bool IsReplaced(string? status) => Equals(Replaced, status);

    public static bool IsCancelled(string? status) => Equals(Cancelled, status);

    public static bool IsDeleted(string? status) => Equals(Deleted, status);

    public static bool IsPartiallyRejected(string? status) => Equals(PartiallyRejected, status);

    public static bool IsSplitConsignment(string? status) => Equals(SplitConsignment, status);

    public static bool IsSubmittedInProgressModify(string? status) => Equals(SubmittedInProgressModify, status);

    private static bool Equals(string status1, string? status2) =>
        status1.Equals(status2, StringComparison.OrdinalIgnoreCase);
}
