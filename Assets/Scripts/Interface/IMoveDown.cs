using System;

public interface IMoveDown
{
    Action OnMoved { get; }
    void MoveDown(IGridObject gridObject);
}