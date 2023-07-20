using System.Collections.Generic;

public interface IGroupable
{
    int GroupIndex { get; }
    int GroupCount { get; }
    HashSet<IGroupable> GroupElements{ get; }
    void AddGroupElement(IGroupable groupable);
    void ResetGroupElements();

}