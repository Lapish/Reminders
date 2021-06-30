using Prism.Ioc;
using Prism.Modularity;
using Reminders.Core.Pipes;
using Reminders.Core.Pipes.Interfaces;
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
            containerProvider.Resolve<IInputService>();
            containerProvider.Resolve<IMessageService>();
            containerProvider.Resolve<IReminderService>();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Services
            containerRegistry.RegisterSingleton<IReminderService, ReminderService>();
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();
            containerRegistry.RegisterSingleton<IInputService, InputService>();
            containerRegistry.RegisterSingleton<ISnackbarService, SnackbarService>();

            //Pipes
            containerRegistry.RegisterSingleton<IPipeClient, PipeClient>();
            containerRegistry.RegisterSingleton<IPipeServer, PipeServer>();           

            //Repositories
            containerRegistry.RegisterSingleton<IReminderRepository, ReminderRepository>();
        }
    }
}