using Prism.Events;

namespace Reminders.Settings.Events
{
    public enum TransitionerContent
    {
        Reminders,
        Settings
    }

    public class ChangedTransitionerContentEvent : PubSubEvent<TransitionerContent>
    {
    }
}