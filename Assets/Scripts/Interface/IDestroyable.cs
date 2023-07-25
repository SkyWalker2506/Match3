using System;

public interface IDestroyable
{
    Action<IGridObject> OnDestroyed { get; }
    void Destroy();
}

public interface IClickable
{
    Action<IGridObject> OnClicked { get; }
}