using Reminders.Core.Models;
using Reminders.Core.Repositories.Interfaces;

namespace Reminders.Core.Repositories
{
    public class ReminderRepository : Repository<Reminder>, IReminderRepository
    {
        public ReminderRepository():base(new ReminderContext())
        {
        }
    }
}
