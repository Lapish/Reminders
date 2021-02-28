using System.Windows.Controls;
using Reminders.Settings.ViewModels;

namespace Reminders.Settings.Views
{
    public partial class ConfigView : UserControl
    {
        public ConfigView()
        {
            InitializeComponent();

            listBox2.OrderChanged += ((ConfigViewModel)DataContext).OrderChanged;
        }
    }
}