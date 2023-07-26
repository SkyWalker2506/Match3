using Match3.NonMono;

namespace Match3.Interface
{
    public interface IDamageSystem
    {
        void ApplyMatchDamage(Grid<IGridObject> grid, IGridObject gridObject);
    }
}