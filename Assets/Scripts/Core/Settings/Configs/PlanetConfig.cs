using UnityEngine;

namespace Core.Planets
{
    [CreateAssetMenu(fileName = "Planet.asset", menuName = "ScriptableObjects/Create planet config", order = 0)]
    public class PlanetConfig : ScriptableObject
    {
        public float    PlanetRadius    = 1.5f;
        public float    MinPlanetsSpeed = 0.02f;
        public float    MaxPlanetsSpeed = 0.2f;
        public int      PlanetHealth    = 100;
        public Material PlayerMaterial;
        public Material EnemyMaterial;
    }
}