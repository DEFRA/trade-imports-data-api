namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class ImportPreNotificationExtensions
{
    public static bool StatusIsDraft(this ImportPreNotification notification) =>
        ImportNotificationStatus.IsDraft(notification.Status);

    public static bool StatusIsSubmitted(this ImportPreNotification notification) =>
        ImportNotificationStatus.IsSubmitted(notification.Status);

    public static bool StatusIsValidated(this ImportPreNotification notification) =>
        ImportNotificationStatus.IsValidated(notification.Status);

    public static bool StatusIsRejected(this ImportPreNotification notification) =>
        ImportNotificationStatus.IsRejected(notification.Status);

    public static bool StatusIsInProgress(this ImportPreNotification notification) =>
        ImportNotificationStatus.IsInProgress(notification.Status);

    public static bool StatusIsAmend(this ImportPreNotification notification) =>
        ImportNotificationStatus.IsAmend(notification.Status);

    public static bool StatusIsModify(this ImportPreNotification notification) =>
        ImportNotificationStatus.IsModify(notification.Status);

    public static bool StatusIsReplaced(this ImportPreNotification notification) =>
        ImportNotificationStatus.IsReplaced(notification.Status);

    public static bool StatusIsCancelled(this ImportPreNotification notification) =>
        ImportNotificationStatus.IsCancelled(notification.Status);

    public static bool StatusIsDeleted(this ImportPreNotification notification) =>
        ImportNotificationStatus.IsDeleted(notification.Status);

    public static bool StatusIsPartiallyRejected(this ImportPreNotification notification) =>
        ImportNotificationStatus.IsPartiallyRejected(notification.Status);

    public static bool StatusIsIsSplitConsignment(this ImportPreNotification notification) =>
        ImportNotificationStatus.IsSplitConsignment(notification.Status);

    public static bool StatusIsSubmittedInProgressModify(this ImportPreNotification notification) =>
        ImportNotificationStatus.IsSubmittedInProgressModify(notification.Status);
}
