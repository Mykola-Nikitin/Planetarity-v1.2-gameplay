using Core.Signals;
using UnityEngine;

namespace Core.Planets
{
    public class PlanetHandler : BasePlanetHandler
    {
        private Camera _camera;

        public PlanetHandler(PlanetComponent planetComponent) : base(planetComponent)
        {
        }

        public override void Destroy(bool isSilent)
        {
            base.Destroy(isSilent);
            
            MessageBus.RemoveListener<PointerDownSignal>(OnPointerDown);
        }

        protected override void Setup()
        {
            base.Setup();
            
            SetMaterial();
            
            MessageBus.AddListener<PointerDownSignal>(OnPointerDown);
        }

        private void SetMaterial()
        {
            PlanetConfig planetConfig = ConfigsService.GetPlanetConfig();
            PlanetComponent.MeshRenderer.sharedMaterial = planetConfig.PlayerMaterial;
        }

        private void OnPointerDown(PointerDownSignal signalData)
        {
            if (RechargeLogic.CanShoot.Value)
            {
                RechargeLogic.StartRecharging();

                Vector3 pointerPosition = MainCamera.ScreenToWorldPoint(signalData.PointerPosition);
                
                RocketsService.CreateRocket(PlanetData.RocketType, PlanetData.ID, PlanetComponent.transform.position, pointerPosition);
            }
        }
    }
}