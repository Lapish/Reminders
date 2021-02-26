using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Reminders.Core.Controls
{
    public class MaterialWindow : Window
    {
        private Button minimizeButton;
        private Button maximizeRestoreButton;
        private Button closeButton;

        private const string MinimizeButtonName = "minimizeButton";
        private const string MaximizeRestoreButtonName = "maximizeRestoreButton";
        private const string CloseButtonName = "closeButton";


        #region Properties

        public Brush MaterialForegroundBrush
        {
            get { return (Brush)GetValue(MaterialForegroundBrushProperty); }
            set { SetValue(MaterialForegroundBrushProperty, value); }
        }
        public static readonly DependencyProperty MaterialForegroundBrushProperty = DependencyProperty.Register(
            nameof(MaterialForegroundBrush), typeof(Brush), typeof(MaterialWindow), new FrameworkPropertyMetadata(null, null));

        public Brush MaterialBackgroundBrush
        {
            get { return (Brush)GetValue(MaterialBackgroundBrushProperty); }
            set { SetValue(MaterialBackgroundBrushProperty, value); }
        }
        public static readonly DependencyProperty MaterialBackgroundBrushProperty = DependencyProperty.Register(
            nameof(MaterialBackgroundBrush), typeof(Brush), typeof(MaterialWindow), new FrameworkPropertyMetadata(null, null));

        public DataTemplate TitleTemplate
        {
            get { return (DataTemplate)GetValue(TitleTemplateProperty); }
            set { SetValue(TitleTemplateProperty, value); }
        }
        public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.Register(
            nameof(TitleTemplate), typeof(DataTemplate), typeof(MaterialWindow));

        public ImageSource TitleIcon
        {
            get { return (ImageSource)GetValue(TitleIconProperty); }
            set { SetValue(TitleIconProperty, value); }
        }
        public static readonly DependencyProperty TitleIconProperty = DependencyProperty.Register(
            nameof(TitleIcon), typeof(ImageSource), typeof(MaterialWindow), new FrameworkPropertyMetadata(null, null));

        #endregion

        public MaterialWindow()
        {
        }

        public override void OnApplyTemplate()
        {
            minimizeButton = GetTemplateChild(MinimizeButtonName) as Button;

            if (minimizeButton != null)
            {
                minimizeButton.Click += MinimizeButtonClickHandler;
            }

            maximizeRestoreButton = GetTemplateChild(MaximizeRestoreButtonName) as Button;

            if (maximizeRestoreButton != null)
            {
                maximizeRestoreButton.Click += MaximizeRestoreButtonClickHandler;
            }

            closeButton = GetTemplateChild(CloseButtonName) as Button;

            if (closeButton != null)
            {
                closeButton.Click += CloseButtonClickHandler;
            }

            base.OnApplyTemplate();
        }

        private void CloseButtonClickHandler(object sender, RoutedEventArgs args)
        {
            Close();
        }

        private void MaximizeRestoreButtonClickHandler(object sender, RoutedEventArgs args)
        {
            WindowState = (WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        private void MinimizeButtonClickHandler(object sender, RoutedEventArgs args)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
