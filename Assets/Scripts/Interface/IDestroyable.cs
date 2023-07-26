using System;

namespace Match3.Interface
{
    public interface IDestroyable
    {
        Action<IGridObject> OnDestroyed { get; set; }
        void Destroy();
    }
}