using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Reminders.Core.Config.Sections;
using System.IO;

namespace Reminders.Core.Config
{
    public sealed class GlobalConfig : ReactiveObject
    {
        public static GlobalConfig Current { get; private set; }

        [JsonProperty("General")]
        public GeneralSection GeneralSection { get; set; }

        [JsonProperty("Reminder")]
        public ReminderSection ReminderSection { get; set; }

        [JsonProperty("NotificationWindow")]
        [Reactive]
        public NotificationWindowSection NotificationWindowSection { get; set; }

        public GlobalConfig()
        {
            GeneralSection = new ();
            ReminderSection = new ();
            NotificationWindowSection = new ();            
        }

        static GlobalConfig()
        {
            if (!File.Exists("config.json"))
            {
                using (StreamWriter sw = File.CreateText("config.json"))
                {
                    JsonSerializer serializer = new();
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(sw, new GlobalConfig());
                }
            }
        }

        public static void TryLoad()
        {
            Current = JsonConvert.DeserializeObject<GlobalConfig>(File.ReadAllText("config.json"));
        }

        public static void Save()
        {
            using (StreamWriter sw = File.CreateText("config.json"))
            {
                JsonSerializer serializer = new ();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(sw, Current);
            }
        }
    }
}