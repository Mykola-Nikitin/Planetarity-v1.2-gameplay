using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Planets;
using Core.Settings;
using UnityEngine.AddressableAssets;

namespace Core.Saves
{
    public class ConfigsService : IConfigsService
    {
        private PlanetConfig           _planetConfig;
        private PlanetGenerationConfig _planetGenerationConfig;
        private RocketConfig[]         _rocketsConfigs;

        public async void Initialize(Action onInitialized)
        {
            await LoadPlanetConfig();
            await LoadPlanetGenerationConfig();
            await LoadRocketsConfigs();
            
            onInitialized?.Invoke();
        }
        
        public PlanetConfig GetPlanetConfig()
        {
            return _planetConfig;
        }

        public PlanetGenerationConfig GetPlanetGenerationConfig()
        {
            return _planetGenerationConfig;
        }

        public RocketConfig GetRocketConfig(int type)
        {
            return _rocketsConfigs.FirstOrDefault(x => x.Type == type);
        }

        public RocketConfig[] GetAllRocketConfigs()
        {
            return _rocketsConfigs;
        }

        private async Task LoadPlanetConfig()
        {
           _planetConfig = await Addressables.LoadAssetAsync<PlanetConfig>("PlanetConfig").Task;
        }

        private async Task LoadPlanetGenerationConfig()
        {
            _planetGenerationConfig = await Addressables.LoadAssetAsync<PlanetGenerationConfig>("PlanetGenerationConfig").Task;
        }

        private async Task LoadRocketsConfigs()
        {
           IList<RocketConfig> configs = await Addressables.LoadAssetsAsync<RocketConfig>("rockets", null).Task;

           _rocketsConfigs = configs.ToArray();
        }
    }
}