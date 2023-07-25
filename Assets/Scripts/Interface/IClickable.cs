using System;

public interface IClickable
{
    Action<IGridObject> OnClicked { get; set; }
}