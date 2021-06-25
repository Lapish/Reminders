using Prism.Ioc;
using Reminders.Notifications.Services.Interfaces;
using Reminders.Notifications.Views;

namespace Reminders.Notifications.Services
{
    public class AmazingDialogService : IAmazingDialogService
    {
        private IContainerExtension _containerExtensions;
        private AmazingWindow _window;

        public AmazingDialogService(IContainerExtension containerExtension)
        {
            _containerExtensions = containerExtension;
        }

        #region Methods

        #region Public

        public void Close()
        {
            _window?.Close();
        }

        public void Show()
        {
            _window = _containerExtensions.Resolve<AmazingWindow>();
            _window.Show();
        }

        #endregion

        #endregion

    }
}