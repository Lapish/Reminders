using ReactiveUI.Fody.Helpers;
using Reminders.Core.MVVM;

namespace Reminders.Core.Controls.Dialogs.ViewModels
{
    public abstract class DialogViewModel : ViewModelBase
    {
        [Reactive]
        public string Title { get; set; }
    }
}