using Microsoft.EntityFrameworkCore;
using Reminders.Core.Models;

namespace Reminders.Core
{
    public class ReminderContext : DbContext
    {
        public DbSet<Reminder> Reminders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
                    => options.UseSqlite("Data Source=Reminders.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangingAndChangedNotificationsWithOriginalValues);
        }
    }
}