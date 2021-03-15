using System;
using System.Collections.Generic;
using System.Linq;
using Core.Saves;
using Core.Signals;
using Core.Utils;
using DI;
using UnityEngine;
using Utils.SignalBus;
using Random = UnityEngine.Random;

namespace Core.Planets
{
    public class PlanetsService : IPlanetsService
    {
        private IConfigsService _configsService;
        private IMessageBus _messageBus;
        private PlanetsModel _planetsModel;

        public void Initialize(Action onInitialized = null)
        {
            _configsService = DIContainer.Get<IConfigsService>();
            _messageBus     = DIContainer.Get<IMessageBus>();
            _planetsModel   = DIContainer.Get<PlanetsModel>();
            
            onInitialized?.Invoke();
        }
        
        public async void GeneratePlanets(Action onFinished)
        {
            bool playerSelected = false;
            
            PlanetGenerationConfig generationConfig = _configsService.GetPlanetGenerationConfig();
            int count = Random.Range(generationConfig.MinPlanetsCount, generationConfig.MaxPlanetsCount + 1);

            for (int i = 0; i < count; i++)
            {
                GameObject planet = await SimplePool.Instance.Get(PoolItemType.Planet);

                if (planet.TryGetComponent(out PlanetComponent planetComponent))
                {
                    bool isPlayer = false;
                    if (!playerSelected)
                    {
                        if (i == count - 1)
                        {
                            isPlayer = true;
                        }
                        else
                        {
                            isPlayer = Random.Range(0, 101) <= 70;
                        }

                        playerSelected |= isPlayer;
                    }
                    
                    planetComponent.Setup(i, isPlayer);
                }
            }
            
            onFinished?.Invoke();
        }

        public async void Load(PlanetData[] planetsData, Action onFinished)
        {
            int count = planetsData.Length;

            for (int i = 0; i < count; i++)
            {
                GameObject planet = await SimplePool.Instance.Get(PoolItemType.Planet);

                if (planet.TryGetComponent(out PlanetComponent planetComponent))
                {
                    planetComponent.Setup(planetsData[i]);
                }
            }
            
            onFinished?.Invoke();
        }

        public void RemovePlanet(GameObject gameObject, bool isSilent)
        {
            SimplePool.Instance.Recycle(PoolItemType.Planet, gameObject);

            if (!isSilent)
            {
                CheckState();
            }
        }

        public void Clear()
        {
            if (_planetsModel.Planets.Count > 0)
            {
                for (int i = _planetsModel.Planets.Count - 1; i >= 0; i--)
                {
                    _planetsModel.Planets[i].Destroy(true);
                }
            }
        }
        
        private void CheckState()
        {
            List<IPlanetHandler> planets = _planetsModel.Planets;

            bool hasPlayer = planets.Any(x => x is PlanetHandler);

            if (hasPlayer)
            {
                if (planets.Count == 1)
                {
                    _messageBus.Fire(new FinishSignal {IsWin = true});
                }
            }
            else
            {
                _messageBus.Fire(new FinishSignal());
            }
        }
    }
}