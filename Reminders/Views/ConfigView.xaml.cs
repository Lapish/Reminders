using System.Windows.Controls;
using Reminders.ViewModels;

namespace Reminders.Views
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