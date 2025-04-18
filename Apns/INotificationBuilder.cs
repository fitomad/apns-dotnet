using Fitomad.Apns.Entities.Notification;

namespace Fitomad.Apns;

public interface INotificationBuilder
{
    /// <summary>
    /// The `alert` property..
    /// </summary>
    /// <param name="alert">You can use an Alert or LocalizedAlert object</param>
    /// <returns></returns>
    INotificationBuilder WithAlert(AlertBase alert);
    /// <summary>
    /// The `badge` key. The number that appear in the app icon badge.
    /// </summary>
    /// <param name="value">The number that appears in the badge</param>
    /// <returns></returns>
    INotificationBuilder WithBadgeCount(int value);
    /// <summary>
    /// A helper method to clear the application icon badge
    /// </summary>
    /// <returns></returns>
    INotificationBuilder ClearBadgeCount();
    /// <summary>
    /// The `sound` property. Plays the default sound.
    /// </summary>
    /// <returns></returns>
    INotificationBuilder PlayDefaultSound();
    /// <summary>
    /// The `sound` property. Plays the sound with the specified name.
    /// </summary>
    /// <param name="value">The sound name</param>
    /// <returns></returns>
    INotificationBuilder PlaySound(string value);
    /// <summary>
    /// The `sound`  property. A more detailed sound information.
    /// </summary>
    /// <param name="value">A custom SoundSettings sound representation</param>
    /// <returns></returns>
    INotificationBuilder PlaySound(SoundSettings value);
    /// <summary>
    /// The `sound` property. A more detailed sound information
    /// </summary>
    /// <param name="name">The sound name you want to play when your certification arrives</param>
    /// <param name="isCritical">Is a critical notification?</param>
    /// <param name="volume">Sets the sound volume</param>
    /// <returns></returns>
    INotificationBuilder PlaySound(string name, bool isCritical, Volume volume);
    /// <summary>
    /// The `thread-id` property. An app-specific identifier for grouping related notifications.
    /// </summary>
    /// <param name="value">The group identifier</param>
    /// <returns></returns>
    INotificationBuilder WithThreadId(string value);
    /// <summary>
    /// The `category` property. It's t notification’s type. This string must correspond to the identifier of one of the UNNotificationCategory objects you register at launch time
    /// </summary>
    /// <param name="value">The notification category</param>
    /// <returns></returns>
    INotificationBuilder WithCategory(string value);
    /// <summary>
    /// The `content-available` property. The background notification flag. Use this method to mark the notification as silent.
    /// </summary>
    /// <returns></returns>
    INotificationBuilder AsSilentNotification();
    /// <summary>
    /// The `mutable-content` property. Allows app extension to modify the notification’s content.
    /// </summary>
    /// <returns></returns>
    INotificationBuilder EnableAppExtensionModification();
    /// <summary>
    /// The `target-content-id` property. The value of this key will be populated on the UNNotificationContent object created from the push payload
    /// </summary>
    /// <param name="value">The identifier of the window brought forward</param>
    /// <returns></returns>
    INotificationBuilder SetTargetContentId(string value);
    /// <summary>
    /// The `interruption-level` property. Sets the importance and delivery timing of a notification
    /// </summary>
    /// <param name="value">Use an InterruptionLevel enumeration value to set the interruption level</param>
    /// <returns></returns>
    INotificationBuilder SetInterruptionLevel(InterruptionLevel value);
    /// <summary>
    /// The `relevance-score` property. A number that the system uses to sort the notifications from your app
    /// </summary>
    /// <param name="value">A number between 0 and 1</param>
    /// <returns></returns>
    INotificationBuilder SetRelevanceScore(double value);
    /// <summary>
    /// The `filter-criteria` property. The criteria the system evaluates to determine if it displays the notification in the current Focus.
    /// </summary>
    /// <param name="value">A string that represents the filter criteria.</param>
    /// <returns></returns>
    INotificationBuilder WithFilterCriteria(string value);
    /// <summary>
    /// Activates the Live Activity notification settings
    /// </summary>
    /// <returns></returns>
    ILiveActivityNotificationBuilder UseLiveActivity();
    /// <summary>
    /// In case you need to restart the notification.
    /// </summary>
    /// <returns></returns>
    INotificationBuilder Reset();
    /// <summary>
    /// Creates a new notification content based on the information you set in the previous methods.
    /// </summary>
    /// <returns>A notification</returns>
    Notification Build();
}