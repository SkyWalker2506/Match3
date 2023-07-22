using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GridData gridData;
    [SerializeReference] private IGridObject IGridObject;
    
    private Grid<IGridObject> grid;
    
    private void Awake()
    {
        CreateGrid();
        Camera.main.orthographicSize = gridData.CellSize * Mathf.Max(gridData.Width, gridData.Height * Camera.main.aspect);
    }

    private void CreateGrid()
    {
        grid = new Grid<IGridObject>(gridData.Width, gridData.Height, gridData.CellSize, new Vector3(gridData.Width-1,gridData.Height-1,0) * gridData.CellSize*-.5f, CreateElement);
    }

    IGridObject CreateElement(Grid<IGridObject> grid, int x, int y)
    {
        IGridObject gridObject = Instantiate(gridData.GetRandomGridObject(),grid.GetWorldPosition(x,y),Quaternion.identity).GetComponent<IGridObject>();
        gridObject.SetSprite(0);
        return gridObject;
    }
}

