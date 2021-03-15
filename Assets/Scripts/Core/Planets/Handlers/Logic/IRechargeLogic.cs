using Core.Utils;

namespace Core.Planets
{
    public interface IRechargeLogic
    {
        Bindable<bool> CanShoot { get; }

        void StartRecharging();

        void Update();
        void Check();
    }
}