using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace Reminders.Core.Config.Sections
{
    [Serializable]
    public class GeneralSection : ReactiveObject, ICloneable
    {
        [JsonProperty("Language")]
        [Reactive]
        public Language Language { get; set; } = Language.Russian;

        public override bool Equals(object obj)
        {
            if (obj is not GeneralSection)
            {
                return false;
            }

            GeneralSection gs = (GeneralSection)obj;
            return gs.Language == Language;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public object Clone()
        {
            return new GeneralSection()
            {
                Language = Language
            };
        }
    }
}