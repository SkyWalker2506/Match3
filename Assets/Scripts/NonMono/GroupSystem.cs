using System.Collections.Generic;
using UnityEngine;

public class GroupSystem: IGroupSystem
{
    private GroupableLists groupableLists;
    
    public void GroupGridObjects(IGridObject[,] gridObjects)
    {
        groupableLists = new GroupableLists();
        if (gridObjects[0, 0]!=null && gridObjects[0, 0].transform.TryGetComponent(out IGroupable firstGroupable))
        {
            groupableLists.AddGroupableList(new GroupableList(firstGroupable));
        }

        for (int i = 0; i < gridObjects.GetLength(0); i++)
        {
            for (int j = 0; j < gridObjects.GetLength(1); j++)
            {
                if (gridObjects[i, j] != null && gridObjects[i, j].transform.TryGetComponent(out IGroupable groupable))
                {
                    if (i > 0)
                    {
                        if(gridObjects[i-1, j] != null && gridObjects[i-1, j].transform.TryGetComponent(out IGroupable groupableLeft))
                        {
                            TryGroupWithNeighbour(groupable, groupableLeft);
                        }
                        else if (groupableLists.GetGroupableList(groupable) == null)
                        {
                            groupableLists.AddGroupableList(new GroupableList(groupable));
                        }
                    }
                    if (j > 0)
                    {
                        if(gridObjects[i, j-1] != null && gridObjects[i, j-1].transform.TryGetComponent(out IGroupable groupableDown))
                        {
                            TryGroupWithNeighbour(groupable, groupableDown);
                        }
                        else if (groupableLists.GetGroupableList(groupable) == null)
                        {
                            groupableLists.AddGroupableList(new GroupableList(groupable));
                        }
                    }
                }
            }
        }
        
        foreach (GroupableList list in groupableLists.Lists)
        {
            foreach (IGroupable groupable in list.Groupables)
            {
                foreach (IGroupable g in list.Groupables)
                {
                    groupable.AddGroupElement(g);
                    g.AddGroupElement(groupable);
                }
            }
        }
    }

    private void TryGroupWithNeighbour(IGroupable groupable, IGroupable groupableNeighbour)
    {
        if (groupable.GroupIndex == groupableNeighbour.GroupIndex)
        {
            GroupableList neighbourList = groupableLists.GetGroupableList(groupableNeighbour);
            if(neighbourList == null)
            {
                neighbourList = new GroupableList(groupableNeighbour);
                groupableLists.AddGroupableList(neighbourList);
            }
            neighbourList.Add(groupable);
        }
        else if (groupableLists.GetGroupableList(groupable) == null)
        {
            groupableLists.AddGroupableList(new GroupableList(groupable));
        }
    }

    class GroupableLists
    {
        public GroupableLists()
        {
            Lists = new List<GroupableList>();
        }

        public List<GroupableList> Lists { get; }

        public GroupableList GetGroupableList(IGroupable groupable)
        {
            return Lists.Find(list => list.ContainsGroupable(groupable));
        }

        public void AddGroupableList(GroupableList groupableList)
        {
            Lists.Add(groupableList);
        }
    }

    class GroupableList
    {
        public HashSet<IGroupable> Groupables { get; }
    
        public GroupableList(IGroupable groupable)
        {
            Groupables = new HashSet<IGroupable> { groupable };
        }
    
        public bool ContainsGroupable(IGroupable groupable)
        {
            return Groupables.Contains(groupable);
        }
        
        public void Add(IGroupable groupable)
        {
            Groupables.Add(groupable);
        }
    
    }
}