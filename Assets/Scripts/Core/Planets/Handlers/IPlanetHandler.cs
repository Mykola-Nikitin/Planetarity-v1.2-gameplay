using UnityEngine;

namespace Core.Planets
{
    public interface IPlanetHandler
    {
        PlanetData PlanetData { get; }
        
        void    Destroy(bool isSilent);
        void    Initialize(int id);
        void    Load(PlanetData data);
        void    Update();
        void    OnTriggerEnter(Collider other);
    }
}