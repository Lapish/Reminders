using System.Windows;

namespace Reminders.Notifications.Views
{
    public partial class Shell : Window
    {
        public Shell()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}