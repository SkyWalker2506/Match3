using System;

public interface IDestroyable
{
    Action<IGridObject> OnDestroyed { get; set; }
    void Destroy();
}