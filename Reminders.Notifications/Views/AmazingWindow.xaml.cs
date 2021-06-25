using System.Threading.Tasks;
using System.Windows;

namespace Reminders.Notifications.Views
{
    public partial class AmazingWindow : Window
    {
        public AmazingWindow()
        {
            InitializeComponent();

            Rect desktopWorkingArea = SystemParameters.WorkArea;
            Left = desktopWorkingArea.Right - Width;
            Top = desktopWorkingArea.Bottom - Height;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(100);
            transitioner.SelectedIndex++;
        }
    }
}