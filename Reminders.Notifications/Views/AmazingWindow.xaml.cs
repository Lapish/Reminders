using MaterialDesignThemes.Wpf.Transitions;
using Prism.Events;
using ReactiveUI.Fody.Helpers;
using Reminders.Core.Config;
using Reminders.Core.Config.Sections;
using Reminders.Core.Transitions;
using Reminders.Notifications.Events;
using Reminders.Notifications.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Reminders.Notifications.Views
{
    public partial class AmazingWindow : Window
    {
        private Rect _desktopWorkingArea;

        public AmazingWindow(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            SetTransitions();

            _desktopWorkingArea = SystemParameters.WorkArea;

            eventAggregator.GetEvent<ConfigChanged>().Subscribe(UpdateWindowSize);

            UpdateWindowSize();
        }

        private void UpdateWindowSize()
        {
            Width = GlobalConfig.Current.NotificationWindowSection.Width;
            Height = GlobalConfig.Current.NotificationWindowSection.Height;

            Left = _desktopWorkingArea.Right - Width;
            Top = _desktopWorkingArea.Bottom - Height;
        }

        private void SetTransitions()
        {
            windowTransition.OpeningEffect = new TransitionEffect(GlobalConfig.Current.NotificationWindowSection.WindowEffect);

            var wipeEffectKind = GlobalConfig.Current.NotificationWindowSection.ContentEffect;

            switch (wipeEffectKind)
            {
                case WipeEffectKind.SlideInFromLeft:
                    contentTransition.ForwardWipe = new SlideWipe() { Direction = SlideDirection.Right };
                    break;

                case WipeEffectKind.SlideInFromTop:
                    contentTransition.ForwardWipe = new SlideWipe() { Direction = SlideDirection.Down };
                    break;

                case WipeEffectKind.SlideInFromRight:
                    contentTransition.ForwardWipe = new SlideWipe() { Direction = SlideDirection.Left };
                    break;

                case WipeEffectKind.SlideInFromBottom:
                    contentTransition.ForwardWipe = new SlideWipe() { Direction = SlideDirection.Up };
                    break;

                case WipeEffectKind.Circle:
                    contentTransition.ForwardWipe = new CircleWipe();
                    break;

                case WipeEffectKind.Fade:
                    contentTransition.ForwardWipe = new FadeWipe();
                    break;
            }            
        }

        protected override void OnClosed(EventArgs e)
        {
            ((AmazingWindowViewModel)DataContext).InfinityUpdateTokenSource.Cancel();
            base.OnClosed(e);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(100);
            transitioner.SelectedIndex++;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        DateTime mouseDown;

        readonly TimeSpan interval = TimeSpan.FromMilliseconds(900);

        private void ListBox_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mouseDown = DateTime.Now;
        }

        private void ListBox_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DateTime.Now.Subtract(mouseDown) > interval)
            {
                transitioner.SelectedIndex++;
            }
            mouseDown = DateTime.Now;
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            transitioner.SelectedIndex--;
        }
    }
}