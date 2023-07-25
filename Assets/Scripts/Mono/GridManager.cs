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
        UpdateGridElements();
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
        UpdateGridElements();
    }
    
    void MoveGridObject(IGridObject gridObject, Vector3 targetPos, float moveTime, Ease moveEase = Ease.Linear)
    {
        gridObject.transform.DOMove(targetPos, moveTime).SetEase(moveEase);
    }

    void UpdateGridElements()
    {
        groupSystem.GroupGridObjects(grid.GridObjects);
        foreach (IGridObject gridObject in grid.GridObjects)
        {
            gridObject?.UpdateSprite();
        }
    }
}

