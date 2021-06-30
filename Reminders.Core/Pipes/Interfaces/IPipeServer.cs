using System;

namespace Reminders.Core.Pipes.Interfaces
{
    public delegate void CommandEventHandler(PipeCommand command);

    public interface IPipeServer
    {
        event EventHandler<PipeCommand> CommandReceived;

        void StartServer();
        void StopServer();
    }
}