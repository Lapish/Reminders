using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using Reminders.Core.Dialogs.ViewModels;
using Reminders.Core.Exceptions;
using Reminders.Core.Services.Interfaces;

namespace Reminders.Core.Services
{
    public class InputService : IInputService
    {
        private InputDialogViewModel _inputDialog;

        public InputService(InputDialogViewModel inputDialog)
        {
            _inputDialog = inputDialog;
        }

        public async Task<string> ShowAsync(string title, string sourceValue)
        {
            _inputDialog.Title = title;
            _inputDialog.Value = sourceValue;

            var result = (bool)await DialogHost.Show(_inputDialog);

            return result ? _inputDialog.Value : throw new InvalidInputException("Text input was canceled by user");
        }
    }
}