using DynamicData;
using Reminders.Core.Models;
using Reminders.Core.Repositories.Interfaces;
using Reminders.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Reminders.Core.Services
{
    public class ReminderService : IReminderService
    {
        private IReminderRepository _reminderRepository;
        private readonly SourceList<Reminder> _reminders = new SourceList<Reminder>();

        public IObservable<IChangeSet<Reminder>> Connect => _reminders.Connect();

        public ReminderService(IReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }

        #region Methods

        #region Public

        public async Task AddAsync(Reminder reminder)
        {
            reminder.Position = _reminders.Count + 1;
            var value = await _reminderRepository.AddAsync(reminder);
            _reminders.Add(value);
        }

        public async Task RenameAsync(int id, string text)
        {
            var reminder = _reminders.Items.FirstOrDefault(x => x.Id.Equals(id));

            _ = reminder ?? throw new ArgumentNullException(nameof(reminder));

            reminder.Text = text;
            var newValue = await _reminderRepository.UpdateAsync(reminder);
            _reminders.Replace(reminder, newValue);
        }

        public bool Unique(string text)
        {
            return _reminders.Items.FirstOrDefault(x => x.Text.Equals(text, StringComparison.OrdinalIgnoreCase)) == null;
        }

        public async Task DeleteAsync(int id)
        {
            var reminder = _reminders.Items.FirstOrDefault(x => x.Id.Equals(id));
            _ = reminder ?? throw new ArgumentNullException(nameof(reminder));

            await _reminderRepository.DeleteAsync(reminder);
            _reminders.Remove(reminder);

            foreach (var sourceReminder in _reminders.Items.Where(x => x.Position > reminder.Position))
            {
                sourceReminder.Position--;
                var newReminder = await _reminderRepository.UpdateAsync(sourceReminder);
                _reminders.Replace(sourceReminder, newReminder);
            }
        }

        public async Task MoveAsync(int sourceIndex, int targetIndex)
        {
            var col = _reminders.Items.OrderBy(x => x.Position).ToList();
            var sourceReminder = col.ElementAt(sourceIndex);
            var targetReminder = col.ElementAt(targetIndex);
            _ = sourceReminder ?? throw new ArgumentNullException(nameof(sourceReminder));
            _ = targetReminder ?? throw new ArgumentNullException(nameof(targetReminder));

            int sourcePos = sourceReminder.Position;

            sourceReminder.Position = targetReminder.Position;
            targetReminder.Position = sourcePos;

            var newSource = await _reminderRepository.UpdateAsync(sourceReminder);
            var newTarget = await _reminderRepository.UpdateAsync(targetReminder);

            _reminders.Edit(r =>
            {
                r.Replace(sourceReminder, newSource);
                r.Replace(targetReminder, newTarget);
            });
        }

        public async Task LoadDataAsync(CancellationToken token)
        {
            var data = await _reminderRepository.GetAllAsync(token);
            _reminders.Edit(r =>
            {
                UpdateData(data);
            });
        }

        #endregion

        #region Private 

        private void UpdateData(IEnumerable<Reminder> newData)
        {
            var addedReminders = new List<Reminder>();

            foreach(var oldReminder in _reminders.Items)
            {
                var reminder = newData.FirstOrDefault(x => x.Id.Equals(oldReminder.Id));

                if (reminder != null)
                {
                    if (!reminder.Equals(oldReminder))
                    {
                        _reminders.Replace(oldReminder, reminder);
                    }
                    else
                    {
                        addedReminders.Add(reminder);
                    }
                }
                else
                {
                    _reminders.Remove(oldReminder);
                }
            }

            var newReminders = newData.Except(addedReminders).Except(_reminders.Items);

            foreach (var newReminder in newReminders)
            {
                _reminders.Add(newReminder);
            }
        }

        #endregion

        #endregion
    }
}