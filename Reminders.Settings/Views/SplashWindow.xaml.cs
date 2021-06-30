using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Reminders.Settings.Views
{
    public partial class SplashWindow : Window
    {
        private CancellationTokenSource _cancellationTokenSource;

        public void RunFadeAnimation(Action completedCallback)
        {
            DoubleAnimation anim = new DoubleAnimation(0, TimeSpan.FromSeconds(1));
            anim.Completed += (s, _) => completedCallback?.Invoke();
            BeginAnimation(OpacityProperty, anim);
        }

        public SplashWindow()
        {
            InitializeComponent();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task ShowError(string error)
        {
            errorTB.Text = error;
            mainGrid.Visibility = Visibility.Collapsed;
            secondGrid.Visibility = Visibility.Visible;

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