using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : IDamageSystem
{
    public void ApplyMatchDamage(Grid<IGridObject> grid, IGridObject gridObject)
    {
        HashSet<IGridObject> gridObjects = new HashSet<IGridObject>();
        if (gridObject.transform.TryGetComponent(out IGroupable groupable))
        {
            foreach (IGridObject element in groupable.GroupElements)
            {
                gridObjects.Add(element);
            }
        }
        if (gridObjects.Count < 2)
        {
            Debug.Log("No Match");
            return;                
        }
        
        HashSet<IGridObject> neighbourGridObjects = GetNeighbourGridObjects(grid, gridObjects);

        foreach (IGridObject go in gridObjects)
        {
            if (go.transform.TryGetComponent(out IDamagable damagable))
            {
                damagable.Damage(1);
            }
        }

        foreach (IGridObject neighbour in neighbourGridObjects)
        {
            if (neighbour.transform.TryGetComponent(out IDamagableFromNeighbour damagable))
            {
                damagable.Damage(1);
            }
        }
        
    }

    private HashSet<IGridObject> GetNeighbourGridObjects(Grid<IGridObject> grid, HashSet<IGridObject> gridObjects)
    {
        HashSet<IGridObject> neighbours = new HashSet<IGridObject>();

        foreach (IGridObject gridObject in gridObjects)
        {
            gridObjects.Add(gridObject);
            int widthIndex = gridObject.WidthIndex;
            int heightIndex = gridObject.HeightIndex;
            if (widthIndex > 0)
            {
                IGridObject left = grid.GridObjects[widthIndex - 1, heightIndex];
                if (left != null)
                {
                    neighbours.Add(left);
                }
            }
            if (widthIndex < grid.GetWidth()-1)
            {
                IGridObject right = grid.GridObjects[widthIndex + 1, heightIndex];
                if (right != null)
                {
                    neighbours.Add(right);
                }
            }
            if (heightIndex > 0)
            {
                IGridObject down = grid.GridObjects[widthIndex, heightIndex-1];
                if (down != null)
                {
                    neighbours.Add(down);
                }
            }
            if (heightIndex <  grid.GetHeight()-1)
            {
                IGridObject up = grid.GridObjects[widthIndex, heightIndex+1];
                if (up != null)
                {
                    neighbours.Add(up);
                }
            }
        }
        
        foreach (IGridObject gridObject in gridObjects)
        {
            if (neighbours.Contains(gridObject))
            {
                neighbours.Remove(gridObject);
            }
        }
        return neighbours;
    }
 
    
}