using DynamicData;
using DynamicData.Binding;
using Prism.Events;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Reminders.Core.Config;
using Reminders.Core.Exceptions;
using Reminders.Core.Models;
using Reminders.Core.MVVM;
using Reminders.Core.Pipes;
using Reminders.Core.Pipes.Interfaces;
using Reminders.Core.Services.Interfaces;
using Reminders.Settings.Events;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Reminders.Settings.ViewModels
{
    public class RemindersViewModel : ViewModelBase
    {
        #region Fields

        private IEventAggregator _eventAggregator;
        private IReminderService _reminderService;
        private IMessageService _messageService;
        private IInputService _inputService;
        private IPipeClient _pipeClient;

        private readonly ReadOnlyObservableCollection<Reminder> _reminders;

        private CancellationTokenSource _infinityUpdateTokenSource { get; set; }

        #endregion

        #region Properties

        [Reactive]
        public int MaxTextLength { get; set; } = GlobalConfig.Current.ReminderSection.MaxTextLength;

        public ReadOnlyObservableCollection<Reminder> Reminders => _reminders;

        [Reactive]
        public Reminder Reminder { get; set; } = new Reminder();

        [Reactive]
        public Reminder SelectedReminder { get; set; }

        [Reactive]
        public bool IsBusy { get; private set; } = true;

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> AddReminder { get; }
        public ReactiveCommand<Unit, Unit> RenameReminder { get; }
        public ReactiveCommand<Unit, Unit> DeleteReminder { get; }
        public bool IsActive { get; set; }

        #endregion

        #region Constructors

        public RemindersViewModel(
            IEventAggregator eventAggregator,
            IReminderService reminderService, 
            IMessageService messageService,
            IInputService inputService,
            IPipeClient pipeClient)
        {
            _eventAggregator = eventAggregator;
            _reminderService = reminderService;
            _messageService = messageService;
            _inputService = inputService;
            _pipeClient = pipeClient;

            _reminderService
                .Connect
                .Sort(SortExpressionComparer<Reminder>.Ascending(x => x.Position))
                .ObserveOnDispatcher()
                .Bind(out _reminders)
                .Subscribe();

            _reminderService
                .Connect
                .Subscribe(_ => OnReminderChanged());                 

            var canAdd = this
                .WhenAnyValue(x => x.Reminder.Text)
                .Select(t => !string.IsNullOrWhiteSpace(t));

            var canDelete = this
                .WhenAnyValue(x => x.SelectedReminder)
                .Select(x => x != null);

            RenameReminder = ReactiveCommand.Create(OnRename);
            AddReminder = ReactiveCommand.Create(OnAddAsync, canAdd);
            DeleteReminder = ReactiveCommand.Create(OnDelete, canDelete);

            _eventAggregator.GetEvent<ChangedTransitionerContentEvent>().Subscribe(OnChangedTransitionerContent);

            StartInfinityUpdateReminders();
        }

        private void OnChangedTransitionerContent(TransitionerContent content)
        {
            if (content == TransitionerContent.Settings)
            {
                _infinityUpdateTokenSource.Cancel();
            }
            _eventAggregator.GetEvent<ChangedTransitionerContentEvent>().Unsubscribe(OnChangedTransitionerContent);
        }

        #endregion

        #region Methods

        #region Public

        public async void OrderChanged(int sourceIndex, int targetIndex)
        {
            await _reminderService.MoveAsync(sourceIndex, targetIndex);
        }

        #endregion

        #region Private

        private async void OnAddAsync()
        {
            if (GlobalConfig.Current.ReminderSection.UniqueTextEnabled &&
                !_reminderService.Unique(Reminder.Text))
            {
                string title = "Добавление напоминания";
                string message = $"Напоминание '{Reminder.Text}' уже существует. Все равно добавить ?";
                if (await _messageService.ShowConfirmationAsync(title, message) == MessageResult.Cancel)
                {
                    return;
                }
            }
            try
            {
                await _reminderService.AddAsync(Reminder);
                Reminder = new Reminder();
            }
            catch (Exception ex)
            {
                await _messageService.ShowInformationAsync("Ошибка добавления", ex.Message);
            }
        }

        private async void OnDelete()
        {
            if (GlobalConfig.Current.ReminderSection.DeleteConfirmationEnabled)
            {
                string title = "Удаление напоминания";
                string message = $"Вы действительно хотите удалить '{SelectedReminder.Text}' ?";

                if (await _messageService.ShowConfirmationAsync(title, message) == MessageResult.Cancel)
                {
                    return;
                }
            }

            try
            {
                await _reminderService.DeleteAsync(SelectedReminder.Id);
            }
            catch (Exception ex)
            {
                await _messageService.ShowInformationAsync("Ошибка удаления", ex.Message);
            }
        }

        private async void OnRename()
        {
            if (SelectedReminder is null)
            {
                return;
            }

            try
            {
                string title = "Переименование напоминания";
                var newText = await _inputService.ShowAsync(
                    title, SelectedReminder.Text, GlobalConfig.Current.ReminderSection.MaxTextLength);
                await _reminderService.RenameAsync(SelectedReminder.Id, newText);
            }
            catch (Exception ex)
            {
                if (!ex.GetBaseException().GetType().Equals(typeof(InvalidInputException)))
                {
                    await _messageService.ShowInformationAsync("Ошибка переименования", ex.Message);
                }
            }
        }

        private async void OnReminderChanged()
        {
            try
            {
                await _pipeClient.SendCommand(PipeCommand.UpdateReminders);
            }
            catch (OperationCanceledException)
            {

            }
        }

        #endregion

        private void StartInfinityUpdateReminders()
        {
            _infinityUpdateTokenSource = new CancellationTokenSource();
            Task.Run(async () =>
            {
                while (!_infinityUpdateTokenSource.IsCancellationRequested)
                {
                    var cts = new CancellationTokenSource();
                    cts.CancelAfter(5000);
                    try
                    {
                        await _reminderService.LoadDataAsync(cts.Token);
                    }
                    catch (OperationCanceledException)
                    {
                    }
                    catch
                    {
                    }
                    IsBusy = false;
                    await Task.Delay(2000);
                }
            });
        }

        #endregion
    }
}