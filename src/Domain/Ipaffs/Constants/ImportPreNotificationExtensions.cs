namespace Defra.TradeImportsDataApi.Domain.Ipaffs.Constants;

public static class ImportPreNotificationExtensions
{
    public static bool StatusIsDraft(this ImportPreNotification notification) =>
        NotificationStatus.IsDraft(notification.Status);

    public static bool StatusIsSubmitted(this ImportPreNotification notification) =>
        NotificationStatus.IsSubmitted(notification.Status);

    public static bool StatusIsValidated(this ImportPreNotification notification) =>
        NotificationStatus.IsValidated(notification.Status);

    public static bool StatusIsRejected(this ImportPreNotification notification) =>
        NotificationStatus.IsRejected(notification.Status);

    public static bool StatusIsInProgress(this ImportPreNotification notification) =>
        NotificationStatus.IsInProgress(notification.Status);

    public static bool StatusIsAmend(this ImportPreNotification notification) =>
        NotificationStatus.IsAmend(notification.Status);

    public static bool StatusIsModify(this ImportPreNotification notification) =>
        NotificationStatus.IsModify(notification.Status);

    public static bool StatusIsReplaced(this ImportPreNotification notification) =>
        NotificationStatus.IsReplaced(notification.Status);

    public static bool StatusIsCancelled(this ImportPreNotification notification) =>
        NotificationStatus.IsCancelled(notification.Status);

    public static bool StatusIsDeleted(this ImportPreNotification notification) =>
        NotificationStatus.IsDeleted(notification.Status);

    public static bool StatusIsPartiallyRejected(this ImportPreNotification notification) =>
        NotificationStatus.IsPartiallyRejected(notification.Status);

    public static bool StatusIsIsSplitConsignment(this ImportPreNotification notification) =>
        NotificationStatus.IsSplitConsignment(notification.Status);

    public static bool StatusIsSubmittedInProgressModify(this ImportPreNotification notification) =>
        NotificationStatus.IsSubmittedInProgressModify(notification.Status);
}
