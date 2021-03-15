using System;
using System.Collections.Generic;
using Core.Planets;
using Core.Saves;
using Core.Weapons;
using DI;

namespace Core.Commands.Implementation
{
    public class PrepareServicesCommand : ICommand
    {
        public event Action<ICommand> Finished;

        private int _nonInitializedServices;
        
        public void Execute()
        {
            List<IService> services = new List<IService>();
            
            services.Add(DIContainer.Get<IPlanetsService>());
            services.Add(DIContainer.Get<IRocketsService>());
            services.Add(DIContainer.Get<ISaveLoadService>());
            services.Add(DIContainer.Get<IConfigsService>());

            _nonInitializedServices = services.Count;
            
            InitServices(services);
        }

        private void InitServices(List<IService> services)
        {
            for (var i = 0; i < services.Count; i++)
            {
                services[i].Initialize(OnServiceInitialized);
            }
        }

        private void OnServiceInitialized()
        {
            _nonInitializedServices--;

            if (_nonInitializedServices == 0)
            {
                Finished?.Invoke(this);
            }
        }
    }
}