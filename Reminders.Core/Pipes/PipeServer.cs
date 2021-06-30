using Reminders.Core.Pipes.Interfaces;
using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Reminders.Core.Pipes
{
    public class PipeServer : IPipeServer
    {
        private CancellationTokenSource _cancellationTokenSource;

        public event EventHandler<PipeCommand> CommandReceived;

        #region Methods

        #region Public

        public void StartServer()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(()=> Server(_cancellationTokenSource.Token));
        }

        public void StopServer()
        {
            _cancellationTokenSource.Cancel();
        }

        #endregion

        #region Private

        private async Task Server(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using(var server = new NamedPipeServerStream("remindersGLK", PipeDirection.InOut))
                {
                    await server.WaitForConnectionAsync(cancellationToken);
                    try
                    {
                        using(var sr = new StreamReader(server))
                        {
                            string command = sr.ReadLine();
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                CommandReceived?.Invoke(this, (PipeCommand)Enum.Parse(typeof(PipeCommand), command));
                            });
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }

        #endregion

        #endregion

    }
}