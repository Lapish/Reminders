using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Reminders.Notifications.Controls
{
    [TemplateVisualState(GroupName = "CommonStates", Name = TemplateStateNormal)]
    [TemplateVisualState(GroupName = "CommonStates", Name = TemplateStateMousePressed)]
    [TemplateVisualState(GroupName = "CommonStates", Name = TemplateStateMouseOut)]
    public class AmazingRipple : ContentControl
    {
        public const string TemplateStateNormal = "Normal";
        public const string TemplateStateMousePressed = "MousePressed";
        public const string TemplateStateMouseOut = "MouseOut";

        private static readonly HashSet<AmazingRipple> PressedInstances = new HashSet<AmazingRipple>();

        static AmazingRipple()
        {

            EventManager.RegisterClassHandler(typeof(ContentControl), Mouse.PreviewMouseUpEvent, new MouseButtonEventHandler(MouseButtonEventHandler), true);
            EventManager.RegisterClassHandler(typeof(ContentControl), Mouse.MouseMoveEvent, new MouseEventHandler(MouseMoveEventHandler), true);
            EventManager.RegisterClassHandler(typeof(Popup), Mouse.PreviewMouseUpEvent, new MouseButtonEventHandler(MouseButtonEventHandler), true);
            EventManager.RegisterClassHandler(typeof(Popup), Mouse.MouseMoveEvent, new MouseEventHandler(MouseMoveEventHandler), true);
        }

        public AmazingRipple()
        {
            SizeChanged += OnSizeChanged;
        }

        private static void MouseButtonEventHandler(object sender, MouseButtonEventArgs e)
        {
            foreach (var ripple in PressedInstances)
            {
                // adjust the transition scale time according to the current animated scale
                var scaleTrans = ripple.Template.FindName("ScaleTransform", ripple) as ScaleTransform;
                if (scaleTrans != null)
                {
                    double currentScale = scaleTrans.ScaleX;
                    var newTime = TimeSpan.FromMilliseconds(300 * (1.0 - currentScale));

                    // change the scale animation according to the current scale
                    var scaleXKeyFrame = ripple.Template.FindName("MousePressedToNormalScaleXKeyFrame", ripple) as EasingDoubleKeyFrame;
                    if (scaleXKeyFrame != null)
                    {
                        scaleXKeyFrame.KeyTime = KeyTime.FromTimeSpan(newTime);
                    }
                    var scaleYKeyFrame = ripple.Template.FindName("MousePressedToNormalScaleYKeyFrame", ripple) as EasingDoubleKeyFrame;
                    if (scaleYKeyFrame != null)
                    {
                        scaleYKeyFrame.KeyTime = KeyTime.FromTimeSpan(newTime);
                    }

                }

                VisualStateManager.GoToState(ripple, TemplateStateNormal, true);
            }
            PressedInstances.Clear();
        }

        private static void Sb_Completed(object sender, EventArgs e)
        {

        }


        private static void MouseMoveEventHandler(object sender, MouseEventArgs e)
        {
            foreach (var ripple in PressedInstances.ToList())
            {
                var relativePosition = Mouse.GetPosition(ripple);
                if (relativePosition.X < 0
                    || relativePosition.Y < 0
                    || relativePosition.X >= ripple.ActualWidth
                    || relativePosition.Y >= ripple.ActualHeight)

                {
                    VisualStateManager.GoToState(ripple, TemplateStateMouseOut, true);
                    PressedInstances.Remove(ripple);
                }
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (RippleAssist.GetIsCentered(this))
            {
                var innerContent = (Content as FrameworkElement);

                if (innerContent != null)
                {
                    var position = innerContent.TransformToAncestor(this)
                        .Transform(new Point(0, 0));

                    if (FlowDirection == FlowDirection.RightToLeft)
                        AmazingRippleX = position.X - innerContent.ActualWidth / 2 - AmazingRippleSize / 2;
                    else
                        AmazingRippleX = position.X + innerContent.ActualWidth / 2 - AmazingRippleSize / 2;
                    AmazingRippleY = position.Y + innerContent.ActualHeight / 2 - AmazingRippleSize / 2;
                }
                else
                {
                    AmazingRippleX = ActualWidth / 2 - AmazingRippleSize / 2;
                    AmazingRippleY = ActualHeight / 2 - AmazingRippleSize / 2;
                }
            }
            else
            {
                var point = e.GetPosition(this);
                AmazingRippleX = point.X - AmazingRippleSize / 2;
                AmazingRippleY = point.Y - AmazingRippleSize / 2;
            }

            if (!RippleAssist.GetIsDisabled(this))
            {
                VisualStateManager.GoToState(this, TemplateStateNormal, false);
                VisualStateManager.GoToState(this, TemplateStateMousePressed, true);
                var sb = Template.FindName("hony", this) as Storyboard;
                sb.Completed += Sb_Completed;
                PressedInstances.Add(this);
            }

            base.OnPreviewMouseLeftButtonDown(e);
        }

        private static readonly DependencyPropertyKey AmazingRippleSizePropertyKey =
            DependencyProperty.RegisterReadOnly(
                "AmazingRippleSize", typeof(double), typeof(AmazingRipple),
                new PropertyMetadata(default(double)));

        private static readonly DependencyProperty AmazingRippleSizeProperty =
            AmazingRippleSizePropertyKey.DependencyProperty;

        public double AmazingRippleSize
        {
            get => (double)GetValue(AmazingRippleSizeProperty);
            private set => SetValue(AmazingRippleSizePropertyKey, value);
        }

        private static readonly DependencyPropertyKey AmazingRippleXPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "AmazingRippleX", typeof(double), typeof(AmazingRipple),
                new PropertyMetadata(default(double)));
        private static readonly DependencyProperty AmazingRippleXProperty =
            AmazingRippleXPropertyKey.DependencyProperty;

        public double AmazingRippleX
        {
            get => (double)GetValue(AmazingRippleXProperty);
            private set => SetValue(AmazingRippleXPropertyKey, value);
        }

        private static readonly DependencyPropertyKey AmazingRippleYPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "AmazingRippleY", typeof(double), typeof(AmazingRipple),
                new PropertyMetadata(default(double)));
        private static readonly DependencyProperty AmazingRippleYProperty =
            AmazingRippleYPropertyKey.DependencyProperty;

        public double AmazingRippleY
        {
            get => (double)GetValue(AmazingRippleYProperty);
            private set => SetValue(AmazingRippleYPropertyKey, value);
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            VisualStateManager.GoToState(this, TemplateStateNormal, false);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            var innerContent = (Content as FrameworkElement);

            double width, height;

            if (RippleAssist.GetIsCentered(this) && innerContent != null)
            {
                width = innerContent.ActualWidth;
                height = innerContent.ActualHeight;
            }
            else
            {
                width = sizeChangedEventArgs.NewSize.Width;
                height = sizeChangedEventArgs.NewSize.Height;
            }

            var radius = Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2));

            AmazingRippleSize = 2 * radius * RippleAssist.GetRippleSizeMultiplier(this);
        }
    }
}
