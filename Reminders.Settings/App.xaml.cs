using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Reminders.Core;
using Reminders.Core.Controls.Dialogs.Views;
using Reminders.Core.Pipes;
using Reminders.Core.Pipes.Interfaces;
using Reminders.Settings.Events;
using Reminders.Settings.Views;
using System.Windows;

namespace Reminders.Settings
{
    public partial class App
    {
        private SplashWindow _splashScreen;

        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var regionManager = Container.Resolve<IRegionManager>();
            var eventAggregator = Container.Resolve<IEventAggregator>();
            regionManager.RegisterViewWithRegion(GlobalRegions.RemindersRegion, typeof(RemindersView));
            regionManager.RegisterViewWithRegion(GlobalRegions.SettingsRegion, typeof(SettingsView));
            regionManager.RegisterViewWithRegion(GlobalRegions.SnackbarRegion, typeof(SnackbarView));

            _splashScreen.RunFadeAnimation((v) =>
            {
                _splashScreen.Close();
                eventAggregator.GetEvent<ChangedShellBusyContent>().Publish(false);
            });

            //regionManager.RequestNavigate(GlobalRegions.ContentRegion, nameof(RemindersView), NavigationCompleted);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _splashScreen = new SplashWindow();
            _splashScreen.Show();
            base.OnStartup(e);
        }

        private void NavigationCompleted(NavigationResult result)
        {
            var eventAggregator = Container.Resolve<IEventAggregator>();
            _splashScreen.RunFadeAnimation((v) =>
            {
                _splashScreen.Close();
                eventAggregator.GetEvent<ChangedShellBusyContent>().Publish(false);
            });
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // containerRegistry.RegisterForNavigation<RemindersView, RemindersViewModel>();
            //     containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();

            containerRegistry.RegisterSingleton<IPipeClient, PipeClient>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<CoreModule>();
        }
    }
}