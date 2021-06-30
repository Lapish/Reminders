using Prism.Events;
using Prism.Regions;
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
using System.Reactive.Linq;
using System.Reactive.Subjects;
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
        private IPipeClient _pipeClient;

        #endregion

        #region Properties

        public GeneralSection GeneralSection { get; set; }

        public ReminderSection ReminderSection { get; set; }

        public NotificationWindowSection NotificationWindowSection { get; set; }

        public string License { get; private set; }

        [Reactive]
        public bool NotificationWindowShown { get; set; }

        [Reactive]
        public bool NavigationEnabled { get; set; } = true;

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> ShowHideNotificationWindowCommand { get; set; }
        public ReactiveCommand<Unit, Unit> BackCommand { get; private set; }

        #endregion

        #region Constructors

        public SettingsViewModel(
            IEventAggregator eventAggregator,
            IMessageService messageService,
            IPipeClient pipeClient)
        {
            _eventAggregator = eventAggregator;
            _messageService = messageService;
            _pipeClient = pipeClient;

            GeneralSection = (GeneralSection)GlobalConfig.Current.GeneralSection.Clone();
            ReminderSection = (ReminderSection)GlobalConfig.Current.ReminderSection.Clone();
            NotificationWindowSection = (NotificationWindowSection)GlobalConfig.Current.NotificationWindowSection.Clone();

            GeneralSection
                .Changed
                //.Throttle(TimeSpan.FromMilliseconds(250))
                .Subscribe(async _ => await OnConfigAnyValueChanged());

            ReminderSection
                .Changed
               // .Throttle(TimeSpan.FromMilliseconds(250))
                .Subscribe(async _ => await OnConfigAnyValueChanged());

            NotificationWindowSection
                .Changed
              //  .Throttle(TimeSpan.FromMilliseconds(250))
                .Subscribe(async _ => await OnConfigAnyValueChanged());

            _eventAggregator.GetEvent<NavigationEnabledChangedEvent>().Subscribe((v) => NavigationEnabled = v);

            ShowHideNotificationWindowCommand = ReactiveCommand.CreateFromTask(OnShowHideNotificationWindowExecuteAsync);

            BackCommand = ReactiveCommand.Create(OnBackExecute);

            LoadLicense();
        }

        #endregion

        #region Methods

        #region Private

        private async Task OnConfigAnyValueChanged()
        {
            GlobalConfig.Current.GeneralSection = (GeneralSection)GeneralSection.Clone();
            GlobalConfig.Current.ReminderSection = (ReminderSection)ReminderSection.Clone();
            GlobalConfig.Current.NotificationWindowSection = (NotificationWindowSection)NotificationWindowSection.Clone();

            try
            {
                GlobalConfig.Save();
                await _pipeClient.SendCommand(PipeCommand.UpdateConfig);
            }
            catch
            {

            }

            //while (true)
            //{
            //    _eventAggregator.GetEvent<NavigationEnabledChangedEvent>().Publish(false);
            //    var cancellationTokenSource = new CancellationTokenSource();
            //    _snackBarService.ShowCircle("Соединение", 500, cancellationTokenSource.Token);
            //    try
            //    {
            //        GlobalConfig.Save();
            //        await _pipeClient.SendCommand(PipeCommand.UpdateConfig);
            //        cancellationTokenSource.Cancel();
            //        _snackBarService.Close();
            //        break;
            //    }
            //    catch (OperationCanceledException)
            //    {
            //        _snackBarService.Close();
            //        if (await _messageService.ShowConfirmationAsync("Ошибка соединения с сервисом", "Попробовать снова?") == MessageResult.Cancel)
            //        {
            //            break;
            //        }
            //    }
            //    catch(Exception ex)
            //    {
            //        if (await _messageService.ShowConfirmationAsync(ex.Message, "Попробовать снова?") == MessageResult.Cancel)
            //        {
            //            break;
            //        }
            //    }
            //}
            //_eventAggregator.GetEvent<NavigationEnabledChangedEvent>().Publish(true);
        }

        private void OnBackExecute()
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

                try
                {
                    PipeCommand command = NotificationWindowShown ? PipeCommand.ShowWindow : PipeCommand.CloseWindow;
                    await _pipeClient.SendCommand(command);
                    break;
                }
                catch (OperationCanceledException)
                {
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