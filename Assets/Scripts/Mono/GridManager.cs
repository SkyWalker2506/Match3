using DG.Tweening;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GridData gridData;
    [SerializeField] private float dropHeight = 20;
    [SerializeField] private float dropTime = 1;
    
    private Grid<IGridObject> grid;
    private GroupSystem groupSystem = new GroupSystem();
    private void Awake()
    {
        CreateGrid();
        Camera.main.orthographicSize = gridData.CellSize * Mathf.Max(gridData.Width, gridData.Height * Camera.main.aspect);
    }

    private void CreateGrid()
    {
        grid = new Grid<IGridObject>(gridData.Width, gridData.Height, gridData.CellSize, new Vector3(gridData.Width-1,gridData.Height-1,0) * gridData.CellSize*-.5f, CreateElement);
        groupSystem.GroupGridObjects(grid.GridObjects);
    }

    IGridObject CreateElement(Grid<IGridObject> grid, int x, int y)
    {
        Vector3 targetPos = grid.GetWorldPosition(x, y);
        Vector3 dropPos = new Vector3(targetPos.x, dropHeight,0);
        IGridObject gridObject = Instantiate(gridData.GetRandomGridObject(),dropPos ,Quaternion.identity).GetComponent<IGridObject>();
        gridObject.SetSprite(0);
        MoveGridObject(gridObject, targetPos, dropTime, Ease.OutBounce);
        return gridObject;
    }

    void MoveGridObject(IGridObject gridObject, Vector3 targetPos, float moveTime, Ease moveEase = Ease.Linear)
    {
        gridObject.transform.DOMove(targetPos, moveTime).SetEase(moveEase);
    }
}

