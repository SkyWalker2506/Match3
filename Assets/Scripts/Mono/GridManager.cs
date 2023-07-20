using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GridData gridData;
    
    private Grid<bool> grid;
    
    private void Awake()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Grid<bool>(gridData.Width, gridData.Height, gridData.CellSize, Vector3.zero, CreateElement);
    }

    bool CreateElement(Grid<bool> grid, int x, int y)
    {
        return false;
    }
}

