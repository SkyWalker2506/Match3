using System.Collections.Generic;
using Match3.NonMono;

namespace Match3.Interface
{
    public interface ICreateSystem
    {
        IGridObject CreateRandomGridObject(int width, int height);
        IGridObject CreateRandomMatchObject(int width, int height);
        IGridObject CreateRandomObstacleObject(int width, int height);
        public List<IGridObject> CreateMissingGridObjects();
        void SetGrid(Grid<IGridObject> gameGrid);
    }
}