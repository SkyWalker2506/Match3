using System;

namespace Match3.Interface
{
    public interface IClickable
    {
        Action<IGridObject> OnClicked { get; set; }
    }
}