using System;
using System.Reactive.Linq;
using ReactiveUI;
using Reminders.Core.Config;
using Reminders.Core.MVVM;

namespace Reminders.Settings.ViewModels
{
    public class TestWindowViewModel : ViewModelBase
    {
        public TestWindowViewModel()
        {
            RemindersConfig.Current.WhenAnyValue(x => x.Language).Subscribe(x => Test());
        }

        private void Test()
        {

        }
    }
}