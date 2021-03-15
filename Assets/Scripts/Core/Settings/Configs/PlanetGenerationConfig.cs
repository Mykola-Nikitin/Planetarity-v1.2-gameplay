using UnityEngine;

namespace Core.Planets
{
    [CreateAssetMenu(fileName = "PlanetsSettings.asset", menuName = "ScriptableObjects/Create planet generation config", order = 0)]
    public class PlanetGenerationConfig : ScriptableObject
    {
        public int   MinPlanetsCount        = 3;
        public int   MaxPlanetsCount        = 5;
        public float DistanceFromSun        = 2f;
        public float DistanceBetweenPlanets = 2f;
    }
}