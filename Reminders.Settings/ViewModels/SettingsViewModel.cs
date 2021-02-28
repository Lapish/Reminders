using System.IO;
using System.Reactive;
using System.Reflection;
using Prism.Regions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Reminders.Core.Config;
using Reminders.Core.MVVM;
using Reminders.Settings.Views;

namespace Reminders.Settings.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private IRegionManager _regionManager;

        #region Properties

        [Reactive]
        public Language Language { get; set; }

        public string License { get; private set; }

        [Reactive]
        public bool IsShowNotificationWindow { get; set; }

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> ShowHideNotificationWindowCommand { get; set; }
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; private set; }

        #endregion

        #region Constructors

        public SettingsViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            ShowHideNotificationWindowCommand = ReactiveCommand.Create(OnShowHideNotificationWindowExecute);
            GoBackCommand = ReactiveCommand.Create(OnGoBackExecute);
            LoadLicense();

            RemindersConfig.Current.Language = Language;
        }

        #endregion

        #region Methods

        #region Private

        private void OnGoBackExecute()
        {
            _regionManager.RequestNavigate(GlobalRegions.ContentRegion, nameof(RemindersView));
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

        private void OnShowHideNotificationWindowExecute()
        {

        }

        #endregion

        #endregion
    }
}