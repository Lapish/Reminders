using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Reminders.Core;
using Reminders.Core.Config;
using Reminders.Core.Events;
using Reminders.Core.Services;
using Reminders.Core.Services.Interfaces;
using Reminders.Notifications.Services;
using Reminders.Notifications.Services.Interfaces;
using Reminders.Notifications.Views;
using System;
using System.Threading;
using System.Windows;

namespace Reminders.Notifications
{
    public partial class App
    {
        private static Mutex _mutex = null;

        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _mutex = new Mutex(true, "Reminders.NotificationsGLK", out bool isNewInstance);

            if (!isNewInstance)
            {
                Current.Shutdown();
            }

            TryLoadConfig(async (m) =>
            {
                var errorWindow = new ConfigErrorWindow(m);
                await errorWindow.ShowAsync();
                Current.Shutdown();
            });

            base.OnStartup(e);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAmazingDialogService, AmazingDialogService>();
            containerRegistry.RegisterSingleton<IDependecyService, DependencyService>();
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

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<CoreModule>();
        }
    }
}