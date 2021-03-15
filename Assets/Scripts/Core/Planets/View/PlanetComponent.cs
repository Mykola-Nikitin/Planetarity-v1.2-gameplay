using Core.UI;
using UnityEngine;

namespace Core.Planets
{
    public class PlanetComponent : MonoBehaviour
    {
        public PlanetHUD    Hud;
        public MeshRenderer MeshRenderer;
        public MeshFilter   MeshFilter;

        private IPlanetHandler _planetHandler;

        private void OnTriggerEnter(Collider other)
        {
            _planetHandler?.OnTriggerEnter(other);
        }

        private void FixedUpdate()
        {
            _planetHandler?.Update();
        }

        public void Setup(int id, bool isPlayer)
        {
            SetHandler(isPlayer);

            _planetHandler.Initialize(id);
        }

        public void Setup(PlanetData planetData)
        {
            SetHandler(planetData.IsPlayer);
            _planetHandler.Load(planetData);
        }

        public IPlanetHandler GetHandler()
        {
            return _planetHandler;
        }

        private void SetHandler(bool isPlayer)
        {
            if (isPlayer)
            {
                _planetHandler = new PlanetHandler(this);
            }
            else
            {
                _planetHandler = new AIPlanetHandler(this);
            }
        }
    }
}