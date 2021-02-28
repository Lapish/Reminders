using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Reminders.Core.Config
{
    public enum Language
    {
        Russian,
        English
    }

    public enum NotificationAnimation
    {
        None,
        HorizontalMove,
        VerticalMove,
        Fade
    }

    public enum NotificationPeriod
    {
        AlwaysActive,

        [Description("1 minute")]
        M1,

        [Description("5 minutes")]
        M5,

        [Description("15 minutes")]
        M15,

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


    public sealed class RemindersConfig : ReactiveObject
    {
        private static readonly Lazy<RemindersConfig> current =
            new Lazy<RemindersConfig>();

        public static RemindersConfig Current
        {
            get
            {
                return current.Value;
            }
        }

        [Reactive]
        public Language Language { get; set; } = Language.English;

        [Reactive]
        public bool OnlyUniqueReminders { get; set; } = true;

        [Reactive]
        public bool EnableDeleteConfirmation { get; set; } = true;

        [Reactive]
        [Range(20, 100)]
        public int MaxTextLength { get; set; } = 50;

        [Reactive]
        public NotificationAnimation NotificationAnimation { get; set; } = NotificationAnimation.None;

        [Reactive]
        public NotificationPeriod NotificationPeriod { get; set; } = NotificationPeriod.H1;
    }
}