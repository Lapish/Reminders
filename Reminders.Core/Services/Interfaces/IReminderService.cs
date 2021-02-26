using System;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;
using Reminders.Core.Models;

namespace Reminders.Core.Services.Interfaces
{
    public interface IReminderService
    {
        IObservable<IChangeSet<Reminder>> Connect { get; }

        Task AddAsync(Reminder reminder);
        Task RenameAsync(int id, string text);
        bool Unique(string text);
        Task DeleteAsync(int id);
        Task MoveAsync(int sourceIndex, int targetIndex);
        Task LoadDataAsync(CancellationToken cancellationToken);
    }
}