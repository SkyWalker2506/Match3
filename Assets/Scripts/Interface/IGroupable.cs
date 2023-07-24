using System.Collections.Generic;
using UnityEngine;

public interface IGroupable
{
    Transform transform { get; }
    int GroupIndex { get; }
    int GroupElementCount { get; }
    HashSet<Transform> GroupElements { get; set; }
    void AddGroupElement(IGroupable groupable);
    void ResetGroupElements();
}