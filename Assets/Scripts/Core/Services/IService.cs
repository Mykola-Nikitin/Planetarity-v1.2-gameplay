using System;

namespace Core.Saves
{
    public interface IService
    {
        void Initialize(Action onInitialized);
    }
}