using MaterialDesignThemes.Wpf.Transitions;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Reminders.Core.Transitions;
using System;
using System.ComponentModel.DataAnnotations;

namespace Reminders.Core.Config.Sections
{
    public class NotificationWindowSection : ReactiveObject, ICloneable
    {
        public const int MinRangeWidth = 250;
        public const int MaxRangeWidth = 700;

        public const int MinRangeHeight = 250;
        public const int MaxRangeHeight = 700;

        [JsonProperty("Width")]
        [Range(MinRangeWidth, MaxRangeWidth)]
        [Reactive]
        public int Width { get; set; } = 320;

        [JsonProperty("Height")]
        [Range(MinRangeHeight, MaxRangeHeight)]
        [Reactive]
        public int Height { get; set; } = 520;

        [JsonProperty("WindowEffect")]
        [Reactive]
        public TransitionEffectKind WindowEffect { get; set; } = TransitionEffectKind.SlideInFromRight;

        [JsonProperty("ContentEffect")]
        [Reactive]
        public WipeEffectKind ContentEffect { get; set; } = WipeEffectKind.Fade;

        [JsonProperty("Period")]
        [Reactive]
        public NotificationPeriod Period { get; set; } = NotificationPeriod.H4;

        [JsonProperty("EnableShowAfterStart")]
        [Reactive]
        public bool ShowAfterStartEnabled { get; set; } = true;

        public override bool Equals(object obj)
        {
            if (obj is not NotificationWindowSection)
            {
                return false;
            }

            NotificationWindowSection nws = (NotificationWindowSection)obj;

            return
                nws.Width.Equals(Width) &&
                nws.Height.Equals(Height) &&
                nws.WindowEffect.Equals(WindowEffect) &&
                nws.ContentEffect.Equals(ContentEffect) &&
                nws.Period.Equals(Period) &&
                nws.ShowAfterStartEnabled.Equals(ShowAfterStartEnabled);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            return new NotificationWindowSection()
            {
                Width = Width,
                Height = Height,
                WindowEffect = WindowEffect,
                ContentEffect = ContentEffect,
                Period = Period,
                ShowAfterStartEnabled = ShowAfterStartEnabled
                
            };
        }
    }
}