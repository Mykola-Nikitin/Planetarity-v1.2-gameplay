using System;

namespace Utils.SignalBus
{
    public interface IMessageBus
    {
        void DefineSignal<T>();
        void AddListener<T>(Action<T> callback);
        void RemoveListener<T>(Action<T> callback);
        void Fire<T>(T signalData);
    }
}