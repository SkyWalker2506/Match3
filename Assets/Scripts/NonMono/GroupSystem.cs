using System.Collections.Generic;
using System.Linq;

public class GroupSystem: IGroupSystem
{
    
    public void GroupGridObjects(IGridObject[,] gridObjects)
    {
        foreach (IGridObject gridObject in gridObjects)
        {
            if(gridObject != null && gridObject.transform.TryGetComponent(out IGroupable groupable))
            {
                groupable.ResetGroupElements();
            }
        }

        for (int i = 0; i < gridObjects.GetLength(0); i++)
        {
            for (int j = 0; j < gridObjects.GetLength(1); j++)
            {
                if (gridObjects[i, j] != null && gridObjects[i, j].transform.TryGetComponent(out IGroupable groupable))
                {
                    groupable.AddGroupElement(groupable);

                    if (i > 0)
                    {
                        if(gridObjects[i-1, j] != null && gridObjects[i-1, j].transform.TryGetComponent(out IGroupable groupableLeft))
                        {
                            TryGroupWithNeighbour(groupable, groupableLeft);
                        }
                    }
                    if (j > 0)
                    {
                        if(gridObjects[i, j-1] != null && gridObjects[i, j-1].transform.TryGetComponent(out IGroupable groupableDown))
                        {
                            TryGroupWithNeighbour(groupable, groupableDown);
                        }
                    }
                }
            }
        }
        
        foreach (IGridObject gridObject in gridObjects)
        {
            if(gridObject != null && gridObject.transform.TryGetComponent(out IGroupable groupable))
            {
                IGroupable[] groupElements = groupable.GroupElements.ToArray();
                foreach (IGroupable groupElement in groupElements)
                {
                    MergeGroupElements(groupElement, groupable);
                }
            }
        }
    }

    private void TryGroupWithNeighbour(IGroupable groupable, IGroupable groupableNeighbour)
    {
        if (groupable.GroupIndex == groupableNeighbour.GroupIndex)
        {
            MergeGroupElements(groupable, groupableNeighbour);
        }
    }
    private void MergeGroupElements(IGroupable groupable1, IGroupable groupable2)
    {
        groupable1.AddGroupElements(groupable2.GroupElements);
        groupable2.AddGroupElements(groupable1.GroupElements);
    }

}