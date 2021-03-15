using System;

namespace Core.Utils
{
    public class Bindable<T>
    {
        private T _value;
        private bool _hold;
        
        private event Action<T, T> _onValueChanged = delegate {  };

        public T Value
        {
            get => _value;
            set
            {
                if (!_value.Equals(value))
                {
                    T oldValue = value;
                    _value = value;

                    _onValueChanged(oldValue, _value);
                }
            }
        }

        public Bindable(T value)
        {
            _value = value;
        }

        public Bindable()
        {
            _value = default;
        }

        public void Subscribe(Action<T, T> onValueChanged, bool immediateAssign = false)
        {
            _onValueChanged += onValueChanged;

            if (immediateAssign)
            {
                onValueChanged(_value, _value);
            }
        }

        public void Unsubscribe(Action<T, T> onValueChanged)
        {
            _onValueChanged -= onValueChanged;
        }
    }
}