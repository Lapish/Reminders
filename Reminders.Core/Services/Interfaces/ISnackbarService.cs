using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reminders.Core.Services.Interfaces
{
    public interface ISnackbarService
    {
        event EventHandler<string> Showed;
        event EventHandler Closed;

        void ShowCircle(string message, int millisecondsDelay, CancellationToken cancellationToken);
        void Close();
    }
}