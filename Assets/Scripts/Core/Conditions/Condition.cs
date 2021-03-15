using System;
using Core.Utils;
using Utils.ConditionManager;

namespace Core.Conditions
{
    public class Condition<T> : ICondition
    {
        public event Action Check = delegate { };

        public bool IsDone => _fieldToCheck.Value.Equals(_expectedValue);

        private T           _expectedValue;
        private Bindable<T> _fieldToCheck;

        public Condition(Bindable<T> fieldToCheck, T expectedValue)
        {
            _fieldToCheck  = fieldToCheck;
            _expectedValue = expectedValue;
            
            _fieldToCheck.Subscribe(OnFieldChanged);
        }

        public void OnFieldChanged(T oldValue, T newValue)
        {
            Check();
        }

        public void Dispose()
        {
            _fieldToCheck?.Unsubscribe(OnFieldChanged);
        }
    }
}