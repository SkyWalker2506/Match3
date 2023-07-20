using System;

public interface IDestroyable
{
    Action OnDestroyed { get; }
    void Destroy();
}