using ReactiveUI.Fody.Helpers;

namespace Reminders.Core.Dialogs.ViewModels
{
    public class ConfirmationDialogViewModel : DialogViewModel
    {
        [Reactive]
        public string Message { get; set; }
    }
}