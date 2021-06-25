using System.Diagnostics;
using System.Reactive;
using System.Threading.Tasks;
using Prism.Events;
using Prism.Regions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Reminders.Core.Events;
using Reminders.Core.MVVM;
using Reminders.Settings.Events;
using Reminders.Settings.Views;

namespace Reminders.Settings.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;

        public ReactiveCommand<Unit, Unit> NavigateSettingsCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenGitHubCommand { get; }

        [Reactive]
        public bool IsBusy { get; private set; }

        [Reactive]
        public double Opacity { get; private set; } = 0.0;

        [Reactive]
        public bool IsNavigationEnabled { get; private set; } = true;

        [Reactive]
        public int TransitionerSelectedIndex { get; private set; }

        public ShellViewModel(
            IRegionManager regionManager,
            IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            eventAggregator.GetEvent<NavigationEnabledChangedEvent>().Subscribe((v) => IsNavigationEnabled = v);
            eventAggregator.GetEvent<ChangedShellBusyContent>().Subscribe((v)=> Opacity = 1.0);
            eventAggregator.GetEvent<ChangedTransitionerContentEvent>().Subscribe(OnChangedTransitionerContent);

            NavigateSettingsCommand = ReactiveCommand.Create(OnNavigateSettingsExecute);
            OpenGitHubCommand = ReactiveCommand.Create(OnOpenGitHubExecute);
        }

        private async void OnChangedTransitionerContent(TransitionerContent content)
        {
            IsBusy = true;
            TransitionerSelectedIndex = (int)content;
            await Task.Delay(1000);
            IsBusy = false;
        }

        private void OnNavigateSettingsExecute()
        {
            _eventAggregator.GetEvent<ChangedTransitionerContentEvent>().Publish(TransitionerContent.Settings);
           // _regionManager.RequestNavigate(GlobalRegions.ContentRegion, nameof(SettingsView));
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