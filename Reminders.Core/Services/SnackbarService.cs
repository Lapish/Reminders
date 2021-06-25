using Reminders.Core.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reminders.Core.Services
{
    public class SnackbarService : ISnackbarService
    {
        public event EventHandler<string> Showed;
        public event EventHandler Closed;

        public void ShowCircle(string message, int millisecondsDelay, CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                await Task.Delay(millisecondsDelay);
                if (!cancellationToken.IsCancellationRequested)
                {
                    Showed?.Invoke(this, message);
                }
            }, cancellationToken);
        }

        public void Close()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}