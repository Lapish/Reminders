using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;
using Reminders.Core.Controls.Dialogs.ViewModels;
using Reminders.Core.Services.Interfaces;

namespace Reminders.Core.Services
{
    public class MessageService : IMessageService
    {
        private ConfirmationDialogViewModel _confirmationDialog;
        private InformationDialogViewModel _informationDialog;

        public MessageService(ConfirmationDialogViewModel confirmationDialog, InformationDialogViewModel informationDialog)
        {
            _confirmationDialog = confirmationDialog;
            _informationDialog = informationDialog;
        }

        public async Task ShowInformationAsync(string title, string message)
        {
            _informationDialog.Title = title;
            _informationDialog.Message = message;

            await Application.Current.Dispatcher.Invoke(async () =>
            {
                await DialogHost.Show(_informationDialog);
            });         
        }

        public async Task<MessageResult> ShowConfirmationAsync(string title, string message)
        {
            _confirmationDialog.Title = title;
            _confirmationDialog.Message = message;

            bool result = await Application.Current.Dispatcher.Invoke(async() =>
            {
                return (bool)await DialogHost.Show(_confirmationDialog);
            });

            return result ? MessageResult.OK : MessageResult.Cancel;
        }
    }
}