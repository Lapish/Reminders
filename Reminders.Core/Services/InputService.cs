using MaterialDesignThemes.Wpf;
using Reminders.Core.Controls.Dialogs.ViewModels;
using Reminders.Core.Exceptions;
using Reminders.Core.Services.Interfaces;
using System.Threading.Tasks;

namespace Reminders.Core.Services
{
    public class InputService : IInputService
    {
        private InputDialogViewModel _inputDialog;

        public InputService(InputDialogViewModel inputDialog)
        {
            _inputDialog = inputDialog;
        }

        public async Task<string> ShowAsync(string title, string sourceValue, int maxTextLength)
        {
            _inputDialog.Title = title;
            _inputDialog.Value = sourceValue;
            _inputDialog.MaxTextLength = maxTextLength;

            var result = (bool)await DialogHost.Show(_inputDialog);

            return result ? _inputDialog.Value : throw new InvalidInputException("Text input was canceled by user");
        }
    }
}