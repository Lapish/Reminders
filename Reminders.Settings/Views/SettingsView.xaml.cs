using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Reminders.Settings.Views
{
    public partial class SettingsView : UserControl
    {
        #region Fields

        private bool _menuIsOpen = true;

        #endregion

        public SettingsView()
        {
            InitializeComponent();
        }

        private void ButtonMenu_Click(object sender, RoutedEventArgs e)
        {
            if (_menuIsOpen)
            {
                Storyboard sb = (Storyboard)FindResource("CloseMenu");
                sb.Begin();
            }
            else
            {
                Storyboard sb = (Storyboard)FindResource("OpenMenu");
                sb.Begin();
            }
            _menuIsOpen = !_menuIsOpen;
        }
    }
}
