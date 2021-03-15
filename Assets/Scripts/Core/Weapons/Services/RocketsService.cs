using System;
using Core.Utils;
using DI;
using UnityEngine;

namespace Core.Weapons
{
    public class RocketsService : IRocketsService
    {
        private RocketsModel _rocketsModel;
        
        public void Initialize(Action onInitialized = null)
        {
            _rocketsModel   = DIContainer.Get<RocketsModel>();
            
            onInitialized?.Invoke();
        }
        
        public void RemoveRocket(GameObject gameObject)
        {
            SimplePool.Instance.Recycle(PoolItemType.Rocket, gameObject);
        }

        public async void Load(RocketData[] rocketsData, Action onLoaded)
        {
            for (int i = 0; i < rocketsData.Length; i++)
            {
                GameObject rocketObject = await SimplePool.Instance.Get(PoolItemType.Rocket);

                if (rocketObject.TryGetComponent(out RocketComponent component))
                {
                    component.Setup(rocketsData[i]);
                }
            }
            
            onLoaded?.Invoke();
        }

        public async void CreateRocket(int type, int ownerID, Vector3 ownerPosition, Vector3 lookPosition)
        {
            GameObject rocketObject = await SimplePool.Instance.Get(PoolItemType.Rocket);

            if (rocketObject.TryGetComponent(out RocketComponent component))
            {
                component.Setup(type, ownerID, ownerPosition, lookPosition);
            }
        }

        public void Clear()
        {
            if (_rocketsModel.Rockets.Count > 0)
            {
                for (int i = _rocketsModel.Rockets.Count - 1; i >= 0; i--)
                {
                    _rocketsModel.Rockets[i].Destroy();
                }
            }
        }
    }
}