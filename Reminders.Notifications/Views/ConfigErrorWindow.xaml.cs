using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Reminders.Notifications.Views
{
    public partial class ConfigErrorWindow : Window
    {
        private CancellationTokenSource _cancellationTokenSource;

        public ConfigErrorWindow(string error)
        {
            InitializeComponent();
            _cancellationTokenSource = new CancellationTokenSource();
            errorTB.Text = error;
        }

        public async Task ShowAsync()
        {
            Show();
            await Task.Run(async () =>
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        await Task.Delay(10000, _cancellationTokenSource.Token);
                    }
                    catch
                    {

                    }
                }
            });

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }

        private void ColorZone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
