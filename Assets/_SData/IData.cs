using System;

public interface IData<T>
{
    public T value {
        get;
        set;
    }
    public void LockEvent();
    public void UnlockEvent();
}