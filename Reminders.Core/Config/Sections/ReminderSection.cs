using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Reminders.Core.Config.Sections
{
    public class ReminderSection
    {
        public const int MinRangeTextLength = 20;
        public const int MaxRangeTextLength = 100;

        [JsonProperty("EnableUniqueText")]
        public bool UniqueTextEnabled { get; set; } = true;

        [JsonProperty("EnableDeleteConfirmation")]
        public bool DeleteConfirmationEnabled { get; set; } = true;

        [Range(MinRangeTextLength, MaxRangeTextLength)]
        [JsonProperty("MaxTextLength")]
        public int MaxTextLength { get; set; } = 50;
    }
}