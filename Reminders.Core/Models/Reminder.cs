using System;
using System.ComponentModel.DataAnnotations.Schema;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Reminders.Core.Models
{
    public class Reminder : ReactiveObject
    {
        public int Id { get; private set; }

        [Reactive]
        public int Position { get; set; }

        [Reactive]
        public string Text { get; set; } = string.Empty;

        public Reminder DeepCopy()
        {
            var reminder = new Reminder();
            reminder.Id = Id;
            reminder.Position = Position;
            reminder.Text = Text;

            return reminder;
        }
    }
}
