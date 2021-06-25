using Newtonsoft.Json;
using ReactiveUI;
using Reminders.Core.Config.Sections;
using System;
using System.IO;

namespace Reminders.Core.Config
{
    public sealed class GlobalConfig : ReactiveObject
    {
        private static readonly Lazy<GlobalConfig> current =
            new Lazy<GlobalConfig>();

        public static GlobalConfig Current => current.Value;

        [JsonProperty("General")]
        public GeneralSection GeneralSection { get; set; }

        [JsonProperty("Reminder")]
        public ReminderSection ReminderSection { get; set; }

        [JsonProperty("NotificationWindow")]
        public NotificationWindowSection WindowSection { get; set; }

        public GlobalConfig()
        {
            GeneralSection = new ();
            ReminderSection = new ();
            WindowSection = new ();            
        }

        public void Save()
        {
            using (StreamWriter sw = File.CreateText("config.json"))
            {
                JsonSerializer serializer = new ();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(sw, this);
            }
        }
    }
}