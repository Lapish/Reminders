using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Reminders.Core.Events;
using Reminders.Core.MVVM;
using Reminders.Core.Pipes;
using Reminders.Core.Pipes.Interfaces;
using Reminders.Core.Services.Interfaces;
using Reminders.Settings.Events;
using Reminders.Settings.Views;
using System;
using System.Diagnostics;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

namespace Reminders.Settings.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private IContainerExtension _container;
        private IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;
        private IDependecyService _dependecyService;
        private ISnackbarService _snackbarService;
        private IPipeClient _pipeClient;

        public ReactiveCommand<Unit, Unit> NavigateSettingsCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenGitHubCommand { get; }

        [Reactive]
        public double Opacity { get; private set; } = 0.0;

        [Reactive]
        public bool IsNavigationEnabled { get; private set; } = true;

        [Reactive]
        public bool ShowInTaskbar { get; private set; }

        [Reactive]
        public int TransitionerSelectedIndex { get; private set; } = 2;

        public ShellViewModel(
            IContainerExtension container,
            IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IDependecyService dependecyService)
        {
            _container = container;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _dependecyService = dependecyService;

            eventAggregator.GetEvent<NavigationEnabledChangedEvent>().Subscribe((v) => IsNavigationEnabled = v);
            eventAggregator.GetEvent<ChangedShellBusyContent>().Subscribe((v)=>
            {
                Opacity = 1.0;
                ShowInTaskbar = true;
            });
            eventAggregator.GetEvent<ChangedTransitionerContentEvent>().Subscribe(OnChangedTransitionerContent);

            eventAggregator.GetEvent<AllModulesLoadedEvent>().Subscribe(OnAllModulesLoaded);

            NavigateSettingsCommand = ReactiveCommand.Create(OnNavigateSettingsExecute);
            OpenGitHubCommand = ReactiveCommand.Create(OnOpenGitHubExecute);
        }

        private void OnAllModulesLoaded()
        {
            _eventAggregator.GetEvent<AllModulesLoadedEvent>().Unsubscribe(OnAllModulesLoaded);

            _pipeClient = (IPipeClient)_dependecyService.GetService(typeof(IPipeClient));
            _snackbarService = (ISnackbarService)_dependecyService.GetService(typeof(ISnackbarService));

            StartInfinityConnectService();
        }

        private async void OnChangedTransitionerContent(TransitionerContent content)
        {
            switch (content)
            {
                case TransitionerContent.Reminders:
                    _regionManager.Regions[GlobalRegions.SettingsRegion].RemoveAll();
                    _regionManager.Regions[GlobalRegions.RemindersRegion].Add(_container.Resolve<RemindersView>());
                    break;

                case TransitionerContent.Settings:
                    _regionManager.Regions[GlobalRegions.RemindersRegion].RemoveAll();
                    _regionManager.Regions[GlobalRegions.SettingsRegion].Add(_container.Resolve<SettingsView>());
                    break;
            }

            TransitionerSelectedIndex = (int)content;
            await Task.Delay(1000);
        }

        private void StartInfinityConnectService()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    var cancellationTokenSource = new CancellationTokenSource();
                    _snackbarService.ShowCircle("Соединение с сервисом", 500, cancellationTokenSource.Token);
                    try
                    {
                        await _pipeClient.SendCommand(PipeCommand.TestConnection);
                        cancellationTokenSource.Cancel();
                        _snackbarService.Close();
                    }
                    catch (OperationCanceledException)
                    {

                    }
                    catch
                    {

                    }

                    await Task.Delay(2000);
                }
            });
        }



        private void OnNavigateSettingsExecute()
        {
            _eventAggregator.GetEvent<ChangedTransitionerContentEvent>().Publish(TransitionerContent.Settings);
        }

        private void OnOpenGitHubExecute()
        {
            string url = "https://github.com/Lapish/Reminders";
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };

            Process.Start(psi);
        }
    }
}