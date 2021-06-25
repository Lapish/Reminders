using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Reminders.Core.Transitions
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum WipeEffectKind
    {
        None,
        SlideInFromLeft,
        SlideInFromTop,
        SlideInFromRight,
        SlideInFromBottom,
        Fade,
        Circle,
        SlideOut
    }
}