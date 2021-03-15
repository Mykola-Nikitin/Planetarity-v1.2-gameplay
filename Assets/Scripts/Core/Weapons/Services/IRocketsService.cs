using System;
using Core.Saves;
using UnityEngine;

namespace Core.Weapons
{
    public interface IRocketsService : IService
    {
        void CreateRocket(int type, int ownerID, Vector3 ownerPosition, Vector3 lookPosition);

        void RemoveRocket(GameObject gameObject);

        void Load(RocketData[] rocketsData, Action onLoaded);

        void Clear();
    }
}