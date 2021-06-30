using Prism.Ioc;
using ReactiveUI;
using Reminders.Core.Services.Interfaces;
using Reminders.Notifications.Services.Interfaces;
using Reminders.Notifications.Views;
using System;
using System.Reactive.Linq;

namespace Reminders.Notifications.Services
{
    public class AmazingDialogService : IAmazingDialogService
    {
        #region Fields

        private IContainerExtension _containerExtensions;
        private IReminderService _reminderService;

        private AmazingWindow _window;
        private bool _windowIsOpen;
        private int _countReminders;

        #endregion

        #region Constructors

        public AmazingDialogService(
            IContainerExtension containerExtension,
            IReminderService reminderService)
        {
            _containerExtensions = containerExtension;
            _reminderService = reminderService;

            _reminderService
                .Connect
                .CountChanged()
                .Subscribe(x => 
                { 
                    _countReminders = _countReminders + x.Adds - x.Removes; 
                });
        }

        #endregion

        #region Methods

        #region Public

        public void Close()
        {
            _window?.Close();
            _windowIsOpen = false;
        }

        public void ShowAsync()
        {
            if (!_windowIsOpen && _countReminders > 0)
            {
                _window = _containerExtensions.Resolve<AmazingWindow>();
                _window.Closed += (s, e) => Close();
                _window.Show();
                _windowIsOpen = true;
            }
        }

        #endregion

        #endregion
    }
}