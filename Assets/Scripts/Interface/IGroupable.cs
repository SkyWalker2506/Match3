using System.Collections.Generic;
using UnityEngine;

namespace Match3.Interface
{
    public interface IGroupable
    {
        Transform transform { get; }
        int GroupIndex { get; }
        int GroupElementCount { get; }
        HashSet<IGroupable> GroupElements { get; set; }
        void AddGroupElement(IGroupable groupable);
        void ResetGroupElements();
        void AddGroupElements(HashSet<IGroupable> groupables);
    }
}