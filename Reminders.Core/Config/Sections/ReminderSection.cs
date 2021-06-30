using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace Reminders.Core.Config.Sections
{
    public class ReminderSection : ReactiveObject, ICloneable
    {
        public const int MinRangeTextLength = 20;
        public const int MaxRangeTextLength = 60;

        [JsonProperty("EnableUniqueText")]
        [Reactive]
        public bool UniqueTextEnabled { get; set; } = true;

        [JsonProperty("EnableDeleteConfirmation")]
        [Reactive]
        public bool DeleteConfirmationEnabled { get; set; } = true;

        [Range(MinRangeTextLength, MaxRangeTextLength)]
        [JsonProperty("MaxTextLength")]
        [Reactive]
        public int MaxTextLength { get; set; } = 40;

        public override bool Equals(object obj)
        {
            if (obj is not ReminderSection)
            {
                return false;
            }

            ReminderSection rs = (ReminderSection)obj;

            return
                rs.UniqueTextEnabled.Equals(UniqueTextEnabled) &&
                rs.DeleteConfirmationEnabled.Equals(DeleteConfirmationEnabled) &&
                rs.MaxTextLength.Equals(MaxTextLength);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            return new ReminderSection()
            {
                UniqueTextEnabled = UniqueTextEnabled,
                DeleteConfirmationEnabled = DeleteConfirmationEnabled,
                MaxTextLength = MaxTextLength
            };
        }
    }
}