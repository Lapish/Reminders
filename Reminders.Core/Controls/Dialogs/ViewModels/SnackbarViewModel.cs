using ReactiveUI.Fody.Helpers;
using Reminders.Core.MVVM;
using Reminders.Core.Services.Interfaces;

namespace Reminders.Core.Controls.Dialogs.ViewModels
{
    public class SnackbarViewModel : ViewModelBase
    {
        #region Fields

        private readonly ISnackbarService _snackbarService;

        #endregion

        #region Properties

        [Reactive]
        public bool IsActive { get; private set; }

        [Reactive]
        public string Message { get; private set; }

        #endregion

        #region Constructors

        public SnackbarViewModel(ISnackbarService snackbarService)
        {
            _snackbarService = snackbarService;

            _snackbarService.Showed += (s, e) => 
            {
                Message = e;
                IsActive = true;
            };

            _snackbarService.Closed += (s, e) => IsActive = false;
        }

        #endregion
    }
}