using Core.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class PlanetHUD : MonoBehaviour
    {
        public Image Health;
        public Image Cooldown;

        private int   _maxHealth;
        private float _maxRechargeTime;

        private Bindable<int>   _planetHealthField;
        private Bindable<float> _planetRechargeField;

        public void Initialize(Bindable<int> healthField, Bindable<float> rechargeField, int maxHealth, float rechargeValue)
        {
            _planetHealthField   = healthField;
            _planetRechargeField = rechargeField;

            _maxHealth       = maxHealth;
            _maxRechargeTime = rechargeValue;
            
            _planetHealthField.Subscribe(OnPlanetHealthChanged, true);
            _planetRechargeField.Subscribe(OnPlanetRechargeChanged, true);
        }

        public void Dispose()
        {
            _planetHealthField?.Unsubscribe(OnPlanetHealthChanged);
            _planetRechargeField?.Unsubscribe(OnPlanetRechargeChanged);
        }

        private void OnDestroy()
        {
            Dispose();
        }
        
        private void OnPlanetHealthChanged(int oldValue, int newValue)
        {
            Health.fillAmount = (float) newValue / _maxHealth;
        } 
        
        private void OnPlanetRechargeChanged(float oldValue, float newValue)
        {
            Cooldown.fillAmount = newValue / _maxRechargeTime;
        }
    }
}