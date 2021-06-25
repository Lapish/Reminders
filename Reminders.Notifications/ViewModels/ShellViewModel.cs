using Prism.Events;
using Reminders.Core.Config;
using Reminders.Core.MVVM;
using Reminders.Core.Pipes;
using Reminders.Core.Pipes.Interfaces;
using Reminders.Notifications.Events;
using Reminders.Notifications.Factory;
using Reminders.Notifications.Services.Interfaces;
using System;

namespace Reminders.Notifications.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        #region Fields

        private IDependencyFactory _dependencyFactory;
        private IPipeServer _pipeServer;
        private IAmazingDialogService _amazingDialogService;
        private IEventAggregator _eventAggregator;
        private GlobalConfig _config;
        private Random _rand;

        #endregion

        #region Constructors

        public ShellViewModel(
            IEventAggregator eventAggregator,
            IDependencyFactory dependencyFactory)
        {
            _eventAggregator = eventAggregator;
            _dependencyFactory = dependencyFactory;
            _rand = new Random();

            _config = GlobalConfig.Current;

            _eventAggregator.GetEvent<AllModulesLoaded>().Subscribe(OnAllModulesLoaded);
        }

        #endregion

        #region Methods

        private void OnAllModulesLoaded()
        {
            _pipeServer = _dependencyFactory.GetPipeServer();
            _amazingDialogService = _dependencyFactory.GetAmazingDialogService();
            _pipeServer.StartServer();
            _pipeServer.CommandReceived += OnCommandReceived;

            _eventAggregator.GetEvent<AllModulesLoaded>().Unsubscribe(OnAllModulesLoaded);
        }

        private void OnCommandReceived(object sender, PipeCommand command)
        {
            switch (command)
            {
                case PipeCommand.ShowWindow:
                    _amazingDialogService.Show();
                    break;

                case PipeCommand.CloseWindow:
                    _amazingDialogService.Close();
                    break;

                case PipeCommand.UpdateReminders:
                    break;
            }
        }

        #endregion
    }
}