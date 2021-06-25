using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Reminders.Core.Config.Sections
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Language
    {
        English,
        Russian
    }
}