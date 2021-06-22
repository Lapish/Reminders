using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Reminders.Settings.Views
{
    public partial class SplashWindow : Window
    {
        public void RunFadeAnimation(Action<object> completedCallback)
        {
            //transitioner.SelectedIndex++;
            var anim = new DoubleAnimation(0, TimeSpan.FromSeconds(1));
              anim.Completed += (s, _) => completedCallback?.Invoke(this);
               BeginAnimation(OpacityProperty, anim);
        }

        public SplashWindow()
        {
            InitializeComponent();
        }
    }
}