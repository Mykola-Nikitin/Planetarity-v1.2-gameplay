using System;
using Core.Planets;
using Core.Saves;
using Core.Weapons;
using DI;
using Utils.SignalBus;

namespace Core.Commands.Implementation
{
    public class PrepareDependenciesCommand : ICommand
    {
        public event Action<ICommand> Finished;

        public void Execute()
        {
            DIContainer.For<IMessageBus>().Use<MessageBus>().AsSingleton().Resolve();
            DIContainer.For<IPlanetsService>().Use<PlanetsService>().AsSingleton().Resolve();
            DIContainer.For<IRocketsService>().Use<RocketsService>().AsSingleton().Resolve();
            DIContainer.For<ISaveLoadService>().Use<SaveLoadService>().AsSingleton().Resolve();
            DIContainer.For<IConfigsService>().Use<ConfigsService>().AsSingleton().Resolve();

            DIContainer.AsSingleton<RocketsModel>();
            DIContainer.AsSingleton<PlanetsModel>();
            DIContainer.AsSingleton<GameModel>();
            DIContainer.AsSingleton<SaveLoadModel>();
            
            Finished?.Invoke(this);
        }
    }
}