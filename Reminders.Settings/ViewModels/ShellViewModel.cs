using System.Diagnostics;
using System.Reactive;
using Prism.Regions;
using ReactiveUI;
using Reminders.Core.MVVM;
using Reminders.Settings.Views;

namespace Reminders.Settings.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private IRegionManager _regionManager;

        public ReactiveCommand<Unit, Unit> NavigateSettingsCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenGitHubCommand { get; }

        public ShellViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            NavigateSettingsCommand = ReactiveCommand.Create(OnNavigateSettingsExecute);
            OpenGitHubCommand = ReactiveCommand.Create(OnOpenGitHubExecute);
        }

        private void OnNavigateSettingsExecute()
        {
            _regionManager.RequestNavigate(GlobalRegions.ContentRegion, nameof(SettingsView));
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