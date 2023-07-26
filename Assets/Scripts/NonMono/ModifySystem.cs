using Match3.Interface;

namespace Match3.NonMono
{
    public class ModifySystem : IModifySystem
    {
        private Grid<IGridObject> grid;

        public ModifySystem(Grid<IGridObject> grid)
        {
            this.grid = grid;
        }
        
        public void ReIndexGridObjects()
        {
            IGridObject[,] gridObjects = grid.GridObjects; 
            for (int i = 0; i < gridObjects.GetLength(0); i++)
            {
                for (int j = 0; j < gridObjects.GetLength(1); j++)
                {
                    if (gridObjects[i, j] == null)
                    {
                        for (int k = j+1; k < gridObjects.GetLength(1); k++)
                        {
                            if(gridObjects[i, k] != null)
                            {
                                if (gridObjects[i, k] is not INonMoveable)
                                {
                                    gridObjects[i, j] = gridObjects[i, k];
                                    gridObjects[i, j].WidthIndex = i;
                                    gridObjects[i, j].HeightIndex = j;
                                    gridObjects[i, k] = null;
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}