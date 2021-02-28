using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace Reminders.Settings.Controls
{
    public class MenuItem : ListViewItem
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(MenuItem), new FrameworkPropertyMetadata(null, null));




        public PackIconKind Icon
        {
            get { return (PackIconKind)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(PackIconKind), typeof(MenuItem), new FrameworkPropertyMetadata(null, null));
    }
}
