using Reminders.Core.Controls;
using System.Windows;

namespace Reminders.Settings.Views
{
    public partial class Shell : MaterialWindow
    {
        public Shell()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
        }
    }
}