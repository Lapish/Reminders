using MaterialDesignThemes.Wpf.Transitions;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Reminders.Core.Config.Sections
{
    public class NotificationWindowSection
    {
        public const int MinRangeWidth = 250;
        public const int MaxRangeWidth = 700;

        public const int MinRangeHeight = 250;
        public const int MaxRangeHeight = 700;

        [JsonProperty("Width")]
        [Range(MinRangeWidth, MaxRangeWidth)]
        public int Width { get; set; } = 320;

        [JsonProperty("Height")]
        [Range(MinRangeHeight, MaxRangeHeight)]
        public int Height { get; set; } = 520;

        [JsonProperty("WindowEffect")]
        public TransitionEffectKind WindowEffect { get; set; } = TransitionEffectKind.SlideInFromRight;

        [JsonProperty("ContentEffect")]
        public TransitionEffectKind ContentEffect { get; set; } = TransitionEffectKind.SlideInFromBottom;

        [JsonProperty("Period")]
        public NotificationPeriod Period { get; set; } = NotificationPeriod.H4;
    }
}