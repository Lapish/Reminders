using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Reminders.Core;
using Reminders.Settings.ViewModels;
using Reminders.Settings.Views;

namespace Reminders.Settings
{
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void OnInitialized()
        {
            var regionManager = Container.Resolve<IRegionManager>();
            //regionManager.RegisterViewWithRegion(GlobalRegions.ContentRegion, typeof(RemindersView));
            regionManager.RequestNavigate(GlobalRegions.ContentRegion, nameof(RemindersView));
            base.OnInitialized();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<RemindersView, RemindersViewModel>();
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<CoreModule>();
        }
    }
}