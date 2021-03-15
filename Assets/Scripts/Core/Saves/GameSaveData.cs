using System;
using Core.Planets;
using Core.Weapons;

namespace Core.Saves
{
    [Serializable]
    public class GameSaveData
    {
        public string       Name;
        public PlanetData[] Planets;
        public RocketData[] Rockets;
    }
}