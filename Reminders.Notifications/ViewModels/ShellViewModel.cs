using Prism.Commands;
using Prism.Events;
using Reminders.Core.Config;
using Reminders.Core.Config.Sections;
using Reminders.Core.Events;
using Reminders.Core.MVVM;
using Reminders.Core.Pipes;
using Reminders.Core.Pipes.Interfaces;
using Reminders.Core.Services.Interfaces;
using Reminders.Notifications.Events;
using Reminders.Notifications.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Reminders.Notifications.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        #region Fields

        private IDependecyService _dependencyService;
        private IPipeServer _pipeServer;
        private IAmazingDialogService _amazingDialogService;
        private IEventAggregator _eventAggregator;
        private IReminderService _reminderService;

        private List<CancellationTokenSource> _periodTokens;

        #endregion

        #region Commands

        public DelegateCommand ShowRemindersCommand { get; }
        public DelegateCommand ShowSettingsCommand { get; }
        public DelegateCommand ShowNotificationWindowCommand { get; }

        #endregion

        #region Constructors

        public ShellViewModel(
            IEventAggregator eventAggregator,
            IDependecyService dependecyService)
        {
            _dependencyService = dependecyService;
            _eventAggregator = eventAggregator;

            ShowRemindersCommand = new DelegateCommand(() => RunSettingsApp("Start:RemindersView"));
            ShowSettingsCommand = new DelegateCommand(() => RunSettingsApp("Start:SettingsView"));
            ShowNotificationWindowCommand = new DelegateCommand(() => _amazingDialogService.ShowAsync());

            _periodTokens = new List<CancellationTokenSource>();

            _eventAggregator.GetEvent<AllModulesLoadedEvent>().Subscribe(OnAllModulesLoaded);
        }

        #endregion

        #region Methods

        private void RunSettingsApp(string arguments)
        {
            Process process = new Process();
            process.StartInfo.FileName = "Settings.exe";
            process.StartInfo.Arguments = arguments;
            process.Start();
        }

        private async void OnAllModulesLoaded()
        {
            _pipeServer = (IPipeServer)_dependencyService.GetService(typeof(IPipeServer));
            _reminderService = (IReminderService)_dependencyService.GetService(typeof(IReminderService));
            _amazingDialogService = (IAmazingDialogService)_dependencyService.GetService(typeof(IAmazingDialogService));


            await UpdateReminders();
            ShowFirstNotification();
            ShowPeriodNotifications();

            _pipeServer.StartServer();
            _pipeServer.CommandReceived += OnCommandReceived;            

            _eventAggregator.GetEvent<AllModulesLoadedEvent>().Unsubscribe(OnAllModulesLoaded);
        }

        private void OnCommandReceived(object sender, PipeCommand command)
        {
            switch (command)
            {
                case PipeCommand.ShowWindow:
                    _amazingDialogService.ShowAsync();
                    break;

                case PipeCommand.CloseWindow:
                    _amazingDialogService.Close();
                    break;

                case PipeCommand.UpdateConfig:
                    OnReceiveUpdateConfigCommand();
                    break;

                case PipeCommand.UpdateReminders:
                    OnReceiveUpdateRemindersCommand();
                    break;
            }
        }

        private void ShowFirstNotification()
        {
            if (GlobalConfig.Current.NotificationWindowSection.ShowAfterStartEnabled)
            {
                _amazingDialogService.ShowAsync();
            }
        }

        private void ShowPeriodNotifications()
        {
            var tokenCTS = new CancellationTokenSource();
            _periodTokens.Add(tokenCTS);

            var windowSection = GlobalConfig.Current.NotificationWindowSection;

            Task.Run(async () =>
            {
                while (windowSection.Period != NotificationPeriod.None)
                {
                    if (windowSection.Period == NotificationPeriod.M30)
                    {
                        await Task.Delay(TimeSpan.FromMinutes(30));
                    }
                    else
                    {
                        int hours = int.Parse(windowSection.Period.ToString().Substring(1));
                        await Task.Delay(TimeSpan.FromHours(hours));
                    }
                    
                    if (!tokenCTS.IsCancellationRequested)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            _amazingDialogService.ShowAsync();
                        });
                    }
                }
            }, tokenCTS.Token);
        }

        private void OnReceiveUpdateConfigCommand()
        {
            try
            {
                var oldWindowSection = (NotificationWindowSection)GlobalConfig.Current.NotificationWindowSection.Clone();
                GlobalConfig.TryLoad();
                _eventAggregator.GetEvent<ConfigChanged>().Publish();

                if (!oldWindowSection.Period.Equals(GlobalConfig.Current.NotificationWindowSection.Period))
                {
                    foreach(var token in _periodTokens)
                    {
                        token.Cancel();
                    }
                    _periodTokens.Clear();
                    ShowPeriodNotifications();
                }
            }
            catch
            {

            }
        }

        private async void OnReceiveUpdateRemindersCommand()
        {
            await UpdateReminders();
        }

        private async Task UpdateReminders()
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
        }

        #endregion
    }
}