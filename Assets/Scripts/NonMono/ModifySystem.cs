using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public void ReindexGridObjects()
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

        public void RearrangeDeadlock()
        {
            List<Tuple<int,int>> availableGridIndexes = new List<Tuple<int,int>>();
            List<IGroupable> allGroupables = new List<IGroupable>();
            

            foreach (IGridObject gridObject in grid.GridObjects)
            {
                if (gridObject != null && gridObject.transform.TryGetComponent(out IGroupable groupable))
                {
                    availableGridIndexes.Add(new Tuple<int, int>(gridObject.WidthIndex, gridObject.HeightIndex));
                    allGroupables.Add(groupable);
                }
            }
            while (allGroupables.Count>0)
            {
                List<IGroupable> currentGroupables = allGroupables.FindAll(g => g.GroupIndex == allGroupables[0].GroupIndex);
                
                foreach (IGroupable groupable in currentGroupables)
                {
                    Tuple<int, int> gridIndexes = availableGridIndexes.First();
                    IGridObject gridObject = groupable.transform.GetComponent<IGridObject>();
                    grid.SetGridObject(gridIndexes.Item1,gridIndexes.Item2,gridObject);
                    gridObject.WidthIndex = gridIndexes.Item1;
                    gridObject.HeightIndex = gridIndexes.Item2;
                    availableGridIndexes.Remove(gridIndexes);
                    allGroupables.Remove(groupable);
                }
            }
        }
        
    }
}