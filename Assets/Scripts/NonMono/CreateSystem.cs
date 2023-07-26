using System.Collections.Generic;
using System.Linq;
using Match3.Interface;
using UnityEngine;

namespace Match3.NonMono
{
    public class CreateSystem : ICreateSystem
    {
        private Grid<IGridObject> grid;
        private GameObject[] matchObjects;
        private GameObject[] obstacleObjects;
        private List<GameObject> allGridObjects;


        public CreateSystem( GameObject[] matchObjects, GameObject[] obstacleObjects)
        {
            this.matchObjects = matchObjects;
            this.obstacleObjects = obstacleObjects;
            allGridObjects = matchObjects.ToList();
            allGridObjects.AddRange(obstacleObjects);
        }

        public void SetGrid(Grid<IGridObject> grid)
        {
            this.grid = grid;
        }
    
        public IGridObject CreateRandomGridObject(int width, int height)
        {
            IGridObject gridObject = Object.Instantiate(allGridObjects[Random.Range(0,allGridObjects.Count)]).GetComponent<IGridObject>();
            gridObject.WidthIndex = width;
            gridObject.HeightIndex = height;
            return gridObject;
        }

        public IGridObject CreateRandomMatchObject(int width, int height)
        {
            IGridObject gridObject = Object.Instantiate(matchObjects[Random.Range(0,matchObjects.Length)]).GetComponent<IGridObject>();
            gridObject.WidthIndex = width;
            gridObject.HeightIndex = height;
            return gridObject;
        }

        public IGridObject CreateRandomObstacleObject(int width, int height)
        {
            IGridObject gridObject = Object.Instantiate(matchObjects[Random.Range(0,obstacleObjects.Length)]).GetComponent<IGridObject>();
            gridObject.WidthIndex = width;
            gridObject.HeightIndex = height;
            return gridObject;
        }
    
        public List<IGridObject> CreateMissingGridObjects()
        {
            List<IGridObject> createdGridObjects = new List<IGridObject>(); 
            IGridObject[,] gridObjects = grid.GridObjects; 
            for (int i = 0; i < gridObjects.GetLength(0); i++)
            {
                var height = gridObjects.GetLength(1);
                for (int j = 0; j < height; j++)
                {
                    if (gridObjects[i, height - j - 1] == null)
                    {
                        gridObjects[i, height - j - 1] = CreateRandomMatchObject(i, height - j - 1);
                        createdGridObjects.Add(gridObjects[i, height - j - 1]);
                    }
                    else if (gridObjects[i, height - j - 1]  is INonMoveable)
                    {
                        break;
                    }
                }
            }

            return createdGridObjects;
        }
    
    }
}