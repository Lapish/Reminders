using ReactiveUI.Fody.Helpers;

namespace Reminders.Core.Dialogs.ViewModels
{
    public class InformationDialogViewModel : DialogViewModel
    {
        [Reactive]
        public string Message { get; set; }
    }
}