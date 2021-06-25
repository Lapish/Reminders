using Reminders.Core.Pipes.Interfaces;
using Reminders.Notifications.Services.Interfaces;

namespace Reminders.Notifications.Factory
{
    public interface IDependencyFactory
    {
        IPipeServer GetPipeServer();
        IAmazingDialogService GetAmazingDialogService();
    }
}