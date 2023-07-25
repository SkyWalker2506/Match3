using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateSystem : ICreateSystem
{
    GameObject[] matchObjects;
    GameObject[] obstacleObjects;
    List<GameObject> allGridObjects;

    public CreateSystem(GameObject[] matchObjects, GameObject[] obstacleObjects)
    {
        this.matchObjects = matchObjects;
        this.obstacleObjects = obstacleObjects;
        allGridObjects = matchObjects.ToList();
        allGridObjects.AddRange(obstacleObjects);
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
    
    
    
}