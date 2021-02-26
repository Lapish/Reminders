using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Reminders.Core.Models;

namespace Reminders.Core.Controls
{
    public class ReorderableReminders : ReorderableListBox<Reminder> { }

    public class ReorderableListBox<T> : ListBox where T : class
    {
        public delegate void OrderChangedEventHandler(int sourceIndex, int targetIndex);

        public event OrderChangedEventHandler OrderChanged;

        private Point _dragStartPoint;

        private P FindVisualParent<P>(DependencyObject child)  where P : DependencyObject
        {
            var parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null)
                return null;

            P parent = parentObject as P;
            if (parent != null)
                return parent;

            return FindVisualParent<P>(parentObject);
        }

        public ReorderableListBox()
        {            
            PreviewMouseMove += ReoderableListBox_MouseMove;

            var sourceStyle = Application.Current.FindResource("ReoderableListBox") as Style;
            var newStyle = new Style(typeof(ListBoxItem), sourceStyle);

            newStyle.Setters.Add(new Setter(AllowDropProperty, true));
            newStyle.Setters.Add(new EventSetter(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(ReoderableListBox_PreviewMouseLeftButtonDown)));
            newStyle.Setters.Add(new EventSetter(DropEvent, new DragEventHandler(ReoderableListBox_Drop)));
            ItemContainerStyle = newStyle;
        }

        private void ReoderableListBox_Drop(object sender, DragEventArgs e)
        {
            if (sender is ListBoxItem)
            {
                var source = e.Data.GetData(typeof(T)) as T;
                var target = ((ListBoxItem)(sender)).DataContext as T;

                int sourceIndex = Items.IndexOf(source);
                int targetIndex = Items.IndexOf(target);

                if (sourceIndex != targetIndex && OrderChanged != null)
                {
                    OrderChanged.Invoke(sourceIndex, targetIndex);
                }
            }
        }

        private void ReoderableListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _dragStartPoint = e.GetPosition(null);
        }

        private void ReoderableListBox_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(null);
            Vector diff = _dragStartPoint - point;
            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                var item = FindVisualParent<ListBoxItem>(((DependencyObject)e.OriginalSource));
                if (item != null)
                {
                    DragDrop.DoDragDrop(item, item.DataContext, DragDropEffects.Move);
                }
            }
        }
    }
}