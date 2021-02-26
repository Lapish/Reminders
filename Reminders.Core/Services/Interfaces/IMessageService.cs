using System.Threading.Tasks;

namespace Reminders.Core.Services.Interfaces
{
    public enum MessageResult
    {
        OK,
        Cancel
    }

    public interface IMessageService
    {
        Task ShowInformationAsync(string title, string message);
        Task<MessageResult> ShowConfirmationAsync(string title, string message);
    }
}
