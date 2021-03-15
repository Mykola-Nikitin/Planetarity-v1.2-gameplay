using System;
using Core.Saves;
using UnityEngine;

namespace Core.Planets
{
    public interface IPlanetsService : IService
    {
        void GeneratePlanets(Action onFinished);
        void Load(PlanetData[] planetsData, Action onFinished);

        void RemovePlanet(GameObject gameObject, bool isSilent);
        void Clear();
    }
}