using Reminders.Settings.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Linq;

namespace Reminders.Settings.Views
{
    public partial class SettingsView : UserControl
    {
        #region Fields

        private bool _menuIsOpen = false;

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

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsInitialized)
            {
                var menuItem = e.AddedItems[0] as AmazingListViewItem;
                switch (menuItem.Id)
                {
                    case 0:
                        generalPanel.BringIntoView();
                        break;

                    case 1:
                        remindersPanel.BringIntoView();
                        break;

                    case 2:
                        notificationsPanel.BringIntoView();
                        break;

                    case 3:
                        aboutPanel.BringIntoView();
                        break;
                }
            }
        }
    }
}