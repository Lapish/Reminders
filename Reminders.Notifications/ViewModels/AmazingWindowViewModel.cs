using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Reminders.Core.Config;
using Reminders.Core.Config.Sections;
using Reminders.Core.Models;
using Reminders.Core.MVVM;
using Reminders.Core.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Reminders.Notifications.ViewModels
{
    public class AmazingWindowViewModel : ViewModelBase
    {
        private IReminderService _reminderService;

        [Reactive]
        public bool IsBusy { get; private set; } = true;

        [Reactive]
        public bool ContentLoaded { get; private set; }

        [Reactive]
        public Reminder SelectedReminder { get; set; }

        public CancellationTokenSource InfinityUpdateTokenSource { get; set; }

        private readonly ReadOnlyObservableCollection<Reminder> _reminders;

        public ReactiveCommand<Unit, Unit> DeleteReminderCommand { get; }

        public ReadOnlyObservableCollection<Reminder> Reminders => _reminders;

        public AmazingWindowViewModel(IReminderService reminderService)
        {
            _reminderService = reminderService;

            _reminderService
                .Connect
                .Sort(SortExpressionComparer<Reminder>.Ascending(x => x.Position))
                .ObserveOnDispatcher()
                .Bind(out _reminders)
                .Subscribe();

            DeleteReminderCommand = ReactiveCommand.Create(OnDeleteReminderExecute);

            StartInfinityUpdateReminders();
        }

        private void StartInfinityUpdateReminders()
        {
            InfinityUpdateTokenSource = new CancellationTokenSource();

            Task.Run(async () =>
            {
                while (!InfinityUpdateTokenSource.Token.IsCancellationRequested)
                {
                    var cts = new CancellationTokenSource();
                    cts.CancelAfter(5000);
                    try
                    {
                        await _reminderService.LoadDataAsync(cts.Token);
                        ContentLoaded = true;
                    }
                    catch (OperationCanceledException)
                    {
                        ContentLoaded = false;
                    }
                    catch
                    {
                        ContentLoaded = false;
                    }
                    IsBusy = false;
                    await Task.Delay(2000);
                }
            });
        }

        private async void OnDeleteReminderExecute()
        {
            try
            {
                await _reminderService.DeleteAsync(SelectedReminder.Id);
            }
            catch
            {

            }
        }
    }
}