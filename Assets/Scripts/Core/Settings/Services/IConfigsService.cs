using Core.Planets;
using Core.Settings;

namespace Core.Saves
{
    public interface IConfigsService : IService
    {
        PlanetConfig           GetPlanetConfig();
        PlanetGenerationConfig GetPlanetGenerationConfig();
        RocketConfig           GetRocketConfig(int type);
        RocketConfig[]         GetAllRocketConfigs();
    }
}