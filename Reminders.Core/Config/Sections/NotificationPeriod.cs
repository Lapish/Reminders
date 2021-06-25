using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;

namespace Reminders.Core.Config.Sections
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum NotificationPeriod
    {
        [Description("One time after start")]
        OneTimeAfterStart,

        [Description("30 minutes")]
        M30,

        [Description("1 hour")]
        H1,

        [Description("2 hours")]
        H2,

        [Description("3 hours")]
        H3,

        [Description("4 hours")]
        H4,

        [Description("5 hours")]
        H5,

        [Description("6 hours")]
        H6
    }
}