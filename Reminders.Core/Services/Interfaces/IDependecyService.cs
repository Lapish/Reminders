using System;

namespace Reminders.Core.Services.Interfaces
{
    public interface IDependecyService
    {
        object GetService(Type type);
    }
}