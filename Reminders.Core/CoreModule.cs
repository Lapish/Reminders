using Prism.Ioc;
using Prism.Modularity;
using Reminders.Core.Repositories;
using Reminders.Core.Repositories.Interfaces;
using Reminders.Core.Services;
using Reminders.Core.Services.Interfaces;

namespace Reminders.Core
{
    public class CoreModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Services
            containerRegistry.RegisterSingleton<IReminderService, ReminderService>();
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();
            containerRegistry.RegisterSingleton<IInputService, InputService>();

            //Repositories
            containerRegistry.RegisterSingleton<IReminderRepository, ReminderRepository>();
        }
    }
}