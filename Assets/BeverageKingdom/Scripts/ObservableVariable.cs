using System;

public class ObservableVariable<T>
{
    private T _value;

    public T Value
    {
        get => _value;
        set
        {
            if (!_value?.Equals(value) ?? value != null) // So sánh tránh null exception
            {
                T oldValue = _value;
                _value = value;
                OnValueChanged?.Invoke(oldValue, _value);
            }
        }
    }

    public event Action<T, T> OnValueChanged;

    public ObservableVariable(T initialValue = default)
    {
        _value = initialValue;
    }
}
