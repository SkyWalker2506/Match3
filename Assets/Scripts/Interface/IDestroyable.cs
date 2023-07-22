using System;

public interface IDestroyable
{
    Action<IGridObject> OnDestroyed { get; }
    void Destroy();
}