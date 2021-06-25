using Newtonsoft.Json;

namespace Reminders.Core.Config.Sections
{
    public class GeneralSection
    {
        [JsonProperty("Language")]
        public Language Language { get; set; } = Language.Russian;
    }
}