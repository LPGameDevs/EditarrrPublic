using System;

public class OnValueChangedArgs<T> : EventArgs
{
    public T value;
    public T previousValue;
}
