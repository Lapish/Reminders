using Prism.Events;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Reminders.Core.Config;
using Reminders.Core.Config.Sections;
using Reminders.Core.Events;
using Reminders.Core.MVVM;
using Reminders.Core.Pipes;
using Reminders.Core.Pipes.Interfaces;
using Reminders.Core.Services.Interfaces;
using Reminders.Settings.Events;
using System;
using System.IO;
using System.Reactive;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Reminders.Settings.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        #region Fields

        private IEventAggregator _eventAggregator;
        private IMessageService _messageService;
        private ISnackbarService _snackBarService;
        private IPipeClient _pipeClient;

        private GlobalConfig _config;

        #endregion

        #region Properties

        [Reactive]
        public GeneralSection GeneralSection { get; set; }

        [Reactive]
        public ReminderSection ReminderSection { get; set; }

        [Reactive]
        public NotificationWindowSection NotificationWindowSection { get; set; }

        public string License { get; private set; }

        [Reactive]
        public bool NotificationWindowShown { get; set; }

        [Reactive]
        public bool NavigationEnabled { get; set; } = true;

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> ShowHideNotificationWindowCommand { get; set; }
        public ReactiveCommand<Unit, Unit> SaveCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; private set; }

        #endregion

        #region Constructors

        public SettingsViewModel(
            IEventAggregator eventAggregator, 
            IMessageService messageService,
            ISnackbarService snackbarService,
            IPipeClient pipeClient)
        {
            _eventAggregator = eventAggregator;
            _messageService = messageService;
            _snackBarService = snackbarService;
            _pipeClient = pipeClient;

            _config = GlobalConfig.Current;

            _eventAggregator.GetEvent<NavigationEnabledChangedEvent>().Subscribe((v) => NavigationEnabled = v);

            ShowHideNotificationWindowCommand = ReactiveCommand.CreateFromTask(OnShowHideNotificationWindowExecuteAsync);

            SaveCommand = ReactiveCommand.Create(OnSaveExecute);
            CancelCommand = ReactiveCommand.Create(OnCancelExecute);

            SetupAnyValueChanged();
            LoadLicense();
        }

        #endregion

        #region Methods
        
        #region Private

        private void SetupAnyValueChanged()
        {
            GeneralSection = new GeneralSection();
            ReminderSection = new ReminderSection();
            NotificationWindowSection = new NotificationWindowSection();
            //this.WhenAnyValue(x => x.Language).Subscribe((x) => _config.GeneralSection.Language = x);
            //TransitionEffectKindSelected = TransitionEffectKind.SlideInFromBottom;
            //this.WhenAnyValue(x => x.UniqueReminderEnabled).Subscribe((x) => _config.UniqueReminderEnabled = x);
         //   this.WhenAnyValue(x => x.DeleteConfirmationEnabled).Subscribe((x) => _config.DeleteConfirmationEnabled = x);
        }

        private void OnSaveExecute()
        {
            _config.GeneralSection = GeneralSection;
            _config.Save();
        }

        private void OnCancelExecute()
        {
            _eventAggregator.GetEvent<ChangedTransitionerContentEvent>().Publish(TransitionerContent.Reminders);
        }

        private void LoadLicense()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Reminders.Settings.Resources.License.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                License = reader.ReadToEnd();
            }
        }

        private async Task OnShowHideNotificationWindowExecuteAsync()
        {
            while (true)
            {
                _eventAggregator.GetEvent<NavigationEnabledChangedEvent>().Publish(false);
                var cancellationTokenSource = new CancellationTokenSource();
                _snackBarService.ShowCircle("Соединение", 500, cancellationTokenSource.Token);

                try
                {
                    PipeCommand command = NotificationWindowShown ? PipeCommand.ShowWindow : PipeCommand.CloseWindow;
                    await _pipeClient.SendCommand(command);
                    cancellationTokenSource.Cancel();
                    _snackBarService.Close();
                    break;
                }
                catch (OperationCanceledException)
                {
                    _snackBarService.Close();
                    if (await _messageService.ShowConfirmationAsync("Ошибка соединения с сервисом", "Попробовать снова?") == MessageResult.Cancel)
                    {
                        NotificationWindowShown = !NotificationWindowShown;
                        break;
                    }
                }
            }
            _eventAggregator.GetEvent<NavigationEnabledChangedEvent>().Publish(true);
        }

        #endregion

        #endregion
    }
}