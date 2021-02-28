using System.Windows.Controls;
using Reminders.Settings.ViewModels;

namespace Reminders.Settings.Views
{
    public partial class RemindersView : UserControl
    {
        public RemindersView()
        {
            InitializeComponent();

            listBox2.OrderChanged += ((RemindersViewModel)DataContext).OrderChanged;
        }
    }
}