using Core.Utils;
using UnityEngine;

namespace Core.Planets
{
    public class RechargeLogic : IRechargeLogic
    {
        public Bindable<bool> CanShoot { get; } = new Bindable<bool>(true);

        private bool _recharging;

        private readonly float           _rechargeTime;
        private readonly Bindable<float> _rechargeField;

        public RechargeLogic(Bindable<float> rechargeField, float rechargeTime)
        {
            _rechargeField = rechargeField;
            _rechargeTime  = rechargeTime;
        }

        public void StartRecharging()
        {
            CanShoot.Value       = false;
            _rechargeField.Value = 0f;
            _recharging          = true;
        }

        public void Check()
        {
            if (_rechargeField.Value >= 0f &&
                _rechargeField.Value < 1f)
            {
                CanShoot.Value = false;
                _recharging    = true;
            }
        }

        public void Update()
        {
            if (_recharging)
            {
                _rechargeField.Value += Time.deltaTime;

                if (_rechargeField.Value >= _rechargeTime)
                {
                    _rechargeField.Value = _rechargeTime;

                    _recharging = false;

                    CanShoot.Value = true;
                }
            }
        }
    }
}