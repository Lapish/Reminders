using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Reminders.Core.Exceptions;
using Reminders.Core.Models;
using Reminders.Core.MVVM;
using Reminders.Core.Services.Interfaces;

namespace Reminders.Settings.ViewModels
{
    public class RemindersViewModel : ViewModelBase
    {
        #region Services

        private IReminderService _reminderService;
        private IMessageService _messageService;
        private IInputService _inputService;

        #endregion

        #region Properties

        public int MaxTextLength { get; set; } = 50;

        private readonly ReadOnlyObservableCollection<Reminder> _reminders;
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

        #endregion

        #region Constructors

        public RemindersViewModel(
            IReminderService reminderService, 
            IMessageService messageService,
            IInputService inputService)
        {
            _reminderService = reminderService;
            _messageService = messageService;
            _inputService = inputService;

            _reminderService
                .Connect
                .Sort(SortExpressionComparer<Reminder>.Ascending(x => x.Position))
                .ObserveOnDispatcher()
                .Bind(out _reminders)
                .Subscribe();

            var canAdd = this
                .WhenAnyValue(x => x.Reminder.Text)
                .Select(t => !string.IsNullOrWhiteSpace(t));

            var canDelete = this
                .WhenAnyValue(x => x.SelectedReminder)
                .Select(x => x != null);

            RenameReminder = ReactiveCommand.Create(OnRename);
            AddReminder = ReactiveCommand.Create(OnAddAsync, canAdd);
            DeleteReminder = ReactiveCommand.Create(OnDelete, canDelete);
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
            if (!_reminderService.Unique(Reminder.Text))
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
            string title = "Удаление напоминания";
            string message = $"Вы действительно хотите удалить '{SelectedReminder.Text}' ?";

            if (await _messageService.ShowConfirmationAsync(title, message) == MessageResult.OK)
            {
                try
                {
                    await _reminderService.DeleteAsync(SelectedReminder.Id);
                }
                catch (Exception ex)
                {
                    await _messageService.ShowInformationAsync("Ошибка удаления", ex.Message);
                }
            }
        }

        private async void OnRename()
        {
            try
            {
                string title = "Переименование напоминания";
                var newText = await _inputService.ShowAsync(title, SelectedReminder.Text);
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

        #endregion

        public async void OnLoad()
        {
            while (true)
            {
                var cancellationToken = new CancellationTokenSource(10000).Token;
                try
                {
                    await _reminderService.LoadDataAsync(cancellationToken);
                    IsBusy = false;
                    return;
                }
                catch (Exception ex)
                {
                    if (await _messageService.ShowConfirmationAsync("Ошибка загрузки данных", $"{ex.Message}\nПопробовать снова?") == MessageResult.Cancel)
                    {
                        Application.Current.Shutdown();
                    }
                }
            }
        }

        #endregion
    }
}