using System.Windows;
using Prism.Ioc;
using Reminders.Notifications.Views;

namespace Reminders.Notifications
{
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<NotificationWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}