using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GridData gridData;
    [SerializeReference] private IGridObject IGridObject;
    
    private Grid<IGridObject> grid;
    
    private void Awake()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Grid<IGridObject>(gridData.Width, gridData.Height, gridData.CellSize, Vector3.zero, CreateElement);
    }

    IGridObject CreateElement(Grid<IGridObject> grid, int x, int y)
    {
        return default;
    }
}

