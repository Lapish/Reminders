using System;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Reminders.Core.MVVM;

namespace Reminders.Core.Dialogs.ViewModels
{
    public class InputDialogViewModel : ViewModelBase
    {
        [Reactive]
        public string Title { get; set; }

        [Reactive]
        public string Value { get; set; }

        [Reactive]
        public bool IsSaveEnabled { get; set; }

        public InputDialogViewModel()
        {
            this
                .WhenAnyValue(x => x.Value)
                .Select(x => IsSaveEnabled = !string.IsNullOrWhiteSpace(x))
                .Subscribe();
        }
    }
}
