using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace Reminders.Settings.Controls
{
    public class AmazingListViewItem : ListViewItem
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(AmazingListViewItem), new FrameworkPropertyMetadata(null, null));

        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(AmazingListViewItem), new PropertyMetadata(0));


        public PackIconKind Icon
        {
            get { return (PackIconKind)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(PackIconKind), typeof(AmazingListViewItem), new FrameworkPropertyMetadata(null, null));

    }
}
