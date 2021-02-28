using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using Prism.Mvvm;
using Reminders.Core.Models;
using Reminders.Core.Services.Interfaces;

namespace Reminders.Notifications.ViewModels
{
    public class NotificationViewModel : BindableBase
    {
        private IReminderService _reminderService;

        private readonly ReadOnlyObservableCollection<Reminder> _reminders;
        public ReadOnlyObservableCollection<Reminder> Reminders => _reminders;

        public NotificationViewModel(IReminderService reminderService)
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