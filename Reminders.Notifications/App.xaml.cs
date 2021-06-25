using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Reminders.Core;
using Reminders.Core.Pipes.Interfaces;
using Reminders.Notifications.Events;
using Reminders.Notifications.Factory;
using Reminders.Notifications.Services;
using Reminders.Notifications.Services.Interfaces;
using Reminders.Notifications.Views;
using System.Windows;

namespace Reminders.Notifications
{
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(LocalRegions.NotificationsRegion, typeof(NotificationsView));

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IPipeServer, IPipeServer>();
            containerRegistry.RegisterSingleton<IAmazingDialogService, AmazingDialogService>();
            containerRegistry.RegisterSingleton<IDependencyFactory, DependencyFactory>();
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();

            var eventAggregator = Container.Resolve<IEventAggregator>();
            eventAggregator?.GetEvent<AllModulesLoaded>().Publish();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<CoreModule>();
        }
    }
}