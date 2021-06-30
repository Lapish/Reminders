using Reminders.Core.Pipes.Interfaces;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace Reminders.Core.Pipes
{
    public class PipeClient : IPipeClient
    {
        public async Task SendCommand(PipeCommand command)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(5000);

            using(var client = new NamedPipeClientStream(".", "remindersGLK", PipeDirection.InOut))
            {
                await client.ConnectAsync(cancellationTokenSource.Token);

                using (var sw = new StreamWriter(client))
                {
                    sw.WriteLine(command);
                }
            }
        }
    }
}