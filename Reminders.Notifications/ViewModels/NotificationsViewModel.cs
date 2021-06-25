using DynamicData;
using DynamicData.Binding;
using Prism.Services.Dialogs;
using Reminders.Core.Models;
using Reminders.Core.MVVM;
using Reminders.Core.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace Reminders.Notifications.ViewModels
{
    public class NotificationsViewModel : ViewModelBase
    {
        private IReminderService _reminderService;

        private readonly ReadOnlyObservableCollection<Reminder> _reminders;

        public event Action<IDialogResult> RequestClose;

        public ReadOnlyObservableCollection<Reminder> Reminders => _reminders;

        public string Title => "";

        public NotificationsViewModel(IReminderService reminderService)
        {
            _reminderService = reminderService;

            _reminderService
                .Connect
                .Sort(SortExpressionComparer<Reminder>.Ascending(x => x.Position))
                .ObserveOnDispatcher()
                .Bind(out _reminders)
                .Subscribe();
        }
    }
}