using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Reminders.Core;
using Reminders.Core.Config;
using Reminders.Core.Controls.Dialogs.Views;
using Reminders.Core.Events;
using Reminders.Core.Services;
using Reminders.Core.Services.Interfaces;
using Reminders.Settings.Events;
using Reminders.Settings.Views;
using System;
using System.Threading;
using System.Windows;

namespace Reminders.Settings
{
    public partial class App
    {
        private SplashWindow _splashScreen;
        private StartupEventArgs _arguments;
        private static Mutex _mutex = null;

        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var regionManager = Container.Resolve<IRegionManager>();
            var eventAggregator = Container.Resolve<IEventAggregator>();
            Container.Resolve<ReminderService>();
            regionManager.RegisterViewWithRegion(GlobalRegions.SnackbarRegion, typeof(SnackbarView));

            TryLoadConfig(async (m) =>
            {
                await _splashScreen.ShowError(m);
                Current.Shutdown();
            });

            _splashScreen.RunFadeAnimation(() => 
            {
                _splashScreen.Close();
                eventAggregator.GetEvent<ChangedShellBusyContent>().Publish(false);
                NavigateToView();
            });

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _mutex = new Mutex(true, "Reminders.SettingsGLK", out bool isNewInstance);

            if (!isNewInstance)
            {
                Current.Shutdown();
            }

            _arguments = e;

            _splashScreen = new SplashWindow();
            _splashScreen.Show();

            base.OnStartup(e);
        }

        private void NavigateToView()
        {
            var eventAggregator = Container.Resolve<IEventAggregator>();

            if (_arguments.Args.Length > 0)
            {
                if (_arguments.Args[0].Equals("Start:SettingsView"))
                {
                    eventAggregator.GetEvent<ChangedTransitionerContentEvent>().Publish(TransitionerContent.Settings);
                }
            }
            else
            {
                eventAggregator.GetEvent<ChangedTransitionerContentEvent>().Publish(TransitionerContent.Reminders);
            }
        }

        private void TryLoadConfig(Action<string> errorCallback)
        {
            try
            {
                GlobalConfig.TryLoad();
            }
            catch (Exception ex)
            {
                errorCallback?.Invoke(ex.Message);
            }            
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();

            var eventAggregator = Container.Resolve<IEventAggregator>();
            eventAggregator?.GetEvent<AllModulesLoadedEvent>().Publish();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IDependecyService, DependencyService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<CoreModule>();
        }
    }
}