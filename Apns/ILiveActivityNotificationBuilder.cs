using Fitomad.Apns.Entities.Notification;

namespace Fitomad.Apns;

public interface ILiveActivityNotificationBuilder
{
    /// <summary>
    /// The `timestamp` property. The UNIX timestamp that marks the time when you send the remote notification that updates or ends a Live Activity.
    /// </summary>
    /// <param name="value">A UNIX epoch time representation</param>
    /// <returns></returns>
    ILiveActivityNotificationBuilder SetTimestamp(long value);
    /// <summary>
    /// The `stale-date` property. The UNIX timestamp that represents the date at which a Live Activity becomes stale, or out of date
    /// </summary>
    /// <param name="value">A UNIX epoch time representation</param>
    /// <returns></returns>
    ILiveActivityNotificationBuilder SetLiveActivityStaleTime(long value);
    /// <summary>
    /// The `content-state` property. The updated or final content for a Live Activity. The content of this dictionary must match the data you describe with your custom ActivityAttributes implementation.
    /// </summary>
    /// <param name="value">A dictionary with the activity content information.</param>
    /// <returns></returns>
    ILiveActivityNotificationBuilder WithLiveActivityContent(IDictionary<string, object> value);
    /// <summary>
    /// The `event` property. The string that describes whether you start, update, or end an ongoing Live Activity with the remote push notification.
    /// </summary>
    /// <param name="value">Use a LiveActivityEvent value to set the event type</param>
    /// <returns></returns>
    ILiveActivityNotificationBuilder SetLiveActivityEvent(LiveActivityEvent value);
    /// <summary>
    /// The `dismissal-date` property. Represents the date at which the system ends a Live Activity and removes it from the Dynamic Island and the Lock Screen
    /// </summary>
    /// <param name="value">A UNIX epoch time representation</param>
    /// <returns></returns>
    ILiveActivityNotificationBuilder SetLiveActivityDismissalDate(long value);
    /// <summary>
    /// The `attributes-type` property. A string you use when you start a Live Activity with a remote push notification. It must match the name of the structure that describes the dynamic data that appears in a Live Activity.
    /// </summary>
    /// <param name="typeName">The structure type name</param>
    /// <returns></returns>
    ILiveActivityNotificationBuilder SetLiveActivityAttributeType(string typeName);
    /// <summary>
    /// The `attributes` property. The dictionary that contains data you pass to a Live Activity that you start with a remote push notification.
    /// </summary>
    /// <param name="attributes">A dictionary with the Live Activity start information</param>
    /// <returns></returns>
    ILiveActivityNotificationBuilder SetLiveActivityStartAttributes(IDictionary<string, object> attributes);
    /// <summary>
    /// In case you need to use regular notification methods.
    /// </summary>
    /// <returns></returns>
    INotificationBuilder UseRegularNotification();
    /// <summary>
    /// Generates the notification.
    /// </summary>
    /// <returns></returns>
    Notification Build();
}