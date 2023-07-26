using DG.Tweening;
using UnityEngine;

public class MoveSystem : IMoveSystem 
{
    private Grid<IGridObject> grid;
    private float dropTime;

    public MoveSystem(Grid<IGridObject> grid, float dropTime)
    {
        this.grid = grid;
        this.dropTime = dropTime;
    }
        
    public void MoveGridObjectsToPositions()
    {
        foreach (IGridObject gridObject in grid.GridObjects)
        {
            if (gridObject != null)
            {
                MoveGridObject(gridObject, grid.GetWorldPosition(gridObject.WidthIndex, gridObject.HeightIndex), dropTime,
                    Ease.OutBounce);
            }
        }
    }
    
    public void MoveGridObject(IGridObject gridObject, Vector3 targetPos, float moveTime, Ease moveEase = Ease.Linear)
    {
        gridObject.transform.DOMove(targetPos, moveTime).SetEase(moveEase);
    }
}