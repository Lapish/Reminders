using System.Threading.Tasks;

namespace Reminders.Core.Services.Interfaces
{
    public interface IInputService
    {
        public Task<string> ShowAsync(string title, string sourceValue, int maxTextLength);
    }
}