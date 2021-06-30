using Reminders.Core.Services.Interfaces;
using System;
using Unity;

namespace Reminders.Core.Services
{
    public class DependencyService : IDependecyService
    {
        private readonly IUnityContainer _container;

        public DependencyService(IUnityContainer container)
        {
            _container = container;
        }

        public object GetService(Type type)
        {
            return _container.Resolve(type);
        }
    }
}