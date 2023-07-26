using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GridData gridData;
    private float dropHeight => gridData.DropHeight;
    private float dropTime => gridData.DropTime;
    private Grid<IGridObject> grid;
    private IGroupSystem groupSystem = new GroupSystem();
    private IDamageSystem damageSystem = new DamageSystem();
    private ICreateSystem createSystem;
    
    
    private void Awake()
    {
        createSystem = new CreateSystem(gridData.MatchObjects,gridData.ObstacleObjects);
        CreateGrid();
        Camera.main.orthographicSize = gridData.CellSize * Mathf.Max(gridData.Width, gridData.Height * Camera.main.aspect);
    }
    
    private void CreateGrid()
    {
        grid = new Grid<IGridObject>(gridData.Width, gridData.Height, gridData.CellSize, new Vector3(gridData.Width-1,gridData.Height-1,0) * gridData.CellSize*-.5f, CreateRandomGridObject);
        StartCoroutine(nameof(UpdateGridElements));
    }

    IEnumerator UpdateGridElements()
    {
        ReIndexGridObjects();
        CreateMissingGridObjects();
        foreach (IGridObject gridObject in grid.GridObjects)
        {
            if (gridObject != null)
            {
                MoveGridObject(gridObject, grid.GetWorldPosition(gridObject.WidthIndex, gridObject.HeightIndex), dropTime,Ease.OutBounce);
            }
        }
        groupSystem.GroupGridObjects(grid.GridObjects);
        foreach (IGridObject gridObject in grid.GridObjects)
        {
            gridObject?.UpdateSprite();
        }
        yield return new WaitForFixedUpdate();

    }
    
    private IGridObject CreateRandomGridObject(Grid<IGridObject> grid, int width, int height)
    {
        return CreateGridObject(grid,width,height,createSystem.CreateRandomGridObject);
    }

    private IGridObject CreateRandomMatchObject(Grid<IGridObject> grid, int width, int height)
    {
        return CreateGridObject(grid,width,height,createSystem.CreateRandomMatchObject);
    }
    
    private  IGridObject CreateGridObject(Grid<IGridObject> grid, int width, int height, Func<int, int,IGridObject> CreateMethod)
    {
        IGridObject gridObject = CreateMethod(width, height);
        Vector3 startPos = grid.GetWorldPosition(width, height);
        startPos.y = dropHeight;
        gridObject.transform.position = startPos;
        if (gridObject.transform.TryGetComponent(out IRenderByGroupSize renderByGroupSize))
        {
            renderByGroupSize.RendererLevelLimits = gridData.RendererLevelLimits;
        }
        if (gridObject.transform.TryGetComponent(out IClickable clickable))
        {
            clickable.OnClicked += OnGridObjectClicked;
        }
        if (gridObject.transform.TryGetComponent(out IDestroyable destroyable))
        {
            destroyable.OnDestroyed += OnGridObjectDestroyed;
        }
        return gridObject;
    }
    
    private void OnGridObjectClicked(IGridObject gridObject)
    {
        damageSystem.ApplyMatchDamage(grid,gridObject);
    }

    private void OnGridObjectDestroyed(IGridObject gridObject)
    {
        if (gridObject.transform.TryGetComponent(out IClickable clickable))
        {
            clickable.OnClicked -= OnGridObjectClicked;
        }

        grid.GridObjects[gridObject.WidthIndex, gridObject.HeightIndex] = null;
        StartCoroutine(nameof(UpdateGridElements));
    }
    
    void MoveGridObject(IGridObject gridObject, Vector3 targetPos, float moveTime, Ease moveEase = Ease.Linear)
    {
        gridObject.transform.DOMove(targetPos, moveTime).SetEase(moveEase);
    }

   

    void ReIndexGridObjects()
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
    
    void CreateMissingGridObjects()
    {
        IGridObject[,] gridObjects = grid.GridObjects; 
        for (int i = 0; i < gridObjects.GetLength(0); i++)
        {
            var height = gridObjects.GetLength(1);
            for (int j = 0; j < height; j++)
            {

                if (gridObjects[i, height - j - 1] == null)
                {
                    gridObjects[i, height - j - 1] = CreateRandomMatchObject(grid, i, height - j - 1);

                }
                else if (gridObjects[i, height - j - 1]  is INonMoveable)
                {
                    break;
                }
            }
        }
    }
}

