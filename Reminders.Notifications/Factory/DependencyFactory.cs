using Reminders.Core.Pipes.Interfaces;
using Reminders.Notifications.Services.Interfaces;
using Unity;

namespace Reminders.Notifications.Factory
{
    public class DependencyFactory : IDependencyFactory
    {
        private readonly IUnityContainer _container;

        public DependencyFactory(IUnityContainer container)
        {
            _container = container;
        }

        public IAmazingDialogService GetAmazingDialogService()
        {
            return _container.Resolve<IAmazingDialogService>();
        }

        public IPipeServer GetPipeServer()
        {
            return _container.Resolve<IPipeServer>();
        }
    }
}