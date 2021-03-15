using System.Collections.Generic;
using System.Linq;
using Helpers;
using UnityEngine;
using Utils;

namespace Core.Planets
{
    public class AIPlanetHandler : BasePlanetHandler
    {
        private float _minShootDelay = 1f;
        private float _maxShootDelay = 2.5f;
        
        public AIPlanetHandler(PlanetComponent planetComponent) 
            : base(planetComponent)
        {
           
        }

        public override void Destroy(bool isSilent)
        {
            base.Destroy(isSilent);
            
            Runner.Instance.Remove(Shoot);
        }

        protected override void Setup()
        {
            base.Setup();
            
            SetColor();
            RechargeLogic.CanShoot.Subscribe(OnCanShoot, true);
        }

        private void SetColor()
        {
            PlanetConfig planetConfig = ConfigsService.GetPlanetConfig();
            PlanetComponent.MeshRenderer.sharedMaterial = planetConfig.EnemyMaterial;

            var propBlock = new MaterialPropertyBlock();
            PlanetComponent.MeshRenderer.GetPropertyBlock(propBlock);
            propBlock.SetColor("_BaseColor",  PlanetData.Color);
            PlanetComponent.MeshRenderer.SetPropertyBlock(propBlock);
        }

        private void Shoot()
        {
            Runner.Instance.Remove(Shoot);
            
            RechargeLogic.StartRecharging();

            List<IPlanetHandler> planets = PlanetsModel.Planets;
            planets.Shuffle();

            IPlanetHandler target = planets.FirstOrDefault(x => x.PlanetData.ID != PlanetData.ID);

            if (target == null)
            {
                return;
            }
            
            Vector3 position = target.PlanetData.Position;

            RocketsService.CreateRocket(PlanetData.RocketType, PlanetData.ID, PlanetComponent.transform.position, position);
        }
        
        private void OnCanShoot(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                float delay = Random.Range(_minShootDelay, _maxShootDelay);
                
                Runner.Instance.Run(Shoot, delay);
            }
        }
    }
}