using System.Collections.Generic;
using UnityEngine;

namespace Core.Planets
{
    public class PlanetsModel
    {
        public Vector3 SunPosition;

        public int PlanetsCount { get; set; }

        public Dictionary<int, int> PlanetsRockets { get; } = new Dictionary<int, int>();

        public List<IPlanetHandler> Planets { get; } = new List<IPlanetHandler>();
    }
}