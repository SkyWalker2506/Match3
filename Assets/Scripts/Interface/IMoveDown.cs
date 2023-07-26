using System;

namespace Match3.Interface
{
    public interface IMoveDown
    {
        Action OnMoved { get; }
        void MoveDown(IGridObject gridObject);
    }
}