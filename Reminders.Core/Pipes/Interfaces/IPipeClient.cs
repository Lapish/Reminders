using System.Threading.Tasks;

namespace Reminders.Core.Pipes.Interfaces
{
    public interface IPipeClient
    {
        Task SendCommand(PipeCommand command);
    }
}
