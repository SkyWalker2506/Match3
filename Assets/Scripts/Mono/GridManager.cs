using System.Collections;
using DG.Tweening;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GridData gridData;
    [SerializeField] private float dropHeight = 20;
    [SerializeField] private float dropTime = 1;
    
    private Grid<IGridObject> grid;
    private IGroupSystem groupSystem = new GroupSystem();
    private IDamageSystem damageSystem = new DamageSystem();
    
    
    private void Awake()
    {
        CreateGrid();
        Camera.main.orthographicSize = gridData.CellSize * Mathf.Max(gridData.Width, gridData.Height * Camera.main.aspect);
    }
    
    private void CreateGrid()
    {
        grid = new Grid<IGridObject>(gridData.Width, gridData.Height, gridData.CellSize, new Vector3(gridData.Width-1,gridData.Height-1,0) * gridData.CellSize*-.5f, CreateElement);
        StartCoroutine(nameof(UpdateGridElements));
    }

    IGridObject CreateElement(Grid<IGridObject> grid, int x, int y)
    {
        Vector3 targetPos = grid.GetWorldPosition(x, y);
        Vector3 dropPos = new Vector3(targetPos.x, dropHeight,0);
        IGridObject gridObject = Instantiate(gridData.GetRandomGridObject(),dropPos ,Quaternion.identity).GetComponent<IGridObject>();
        gridObject.WidthIndex = x;
        gridObject.HeightIndex = y;
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
        
        MoveGridObject(gridObject, targetPos, dropTime, Ease.OutBounce);
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
        yield return new WaitForEndOfFrame();
        groupSystem.GroupGridObjects(grid.GridObjects);
        yield return new WaitForEndOfFrame();
        foreach (IGridObject gridObject in grid.GridObjects)
        {
            gridObject?.UpdateSprite();
        }
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
                    gridObjects[i, height - j - 1] = CreateElement(grid, i, height - j - 1);

                }
                else if (gridObjects[i, height - j - 1]  is INonMoveable)
                {
                    break;
                }
            }
        }
    }


    
}

