using System.Collections.Generic;
using Core.Settings;

namespace Core.Weapons
{
    public class RocketsModel
    {
        public RocketConfig[] RocketsConfigs { get; set; }
        
        public List<IRocketHandler> Rockets { get; } = new List<IRocketHandler>();
    }
}