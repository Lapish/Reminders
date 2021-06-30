using System.Windows.Controls;
using System.Windows.Input;

namespace Reminders.Core.Controls.Dialogs.Views
{
    public partial class InputDialog : UserControl
    {
        public InputDialog()
        {
            InitializeComponent();
        }

        private void UserControl_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
            }
        }
    }
}