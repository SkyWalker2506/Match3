using System;
using System.Collections.Generic;
using UnityEngine;

public class MatchObject : GridObject, IGroupable, IDamagable, IDestroyable, IMoveDown
{
    [field: SerializeField] public int GroupIndex { get; private set; }
    public int GroupCount => GroupElements.Count;
    public HashSet<IGroupable> GroupElements { get; private set; }
    public Action<IGridObject> OnDamaged { get; }
    public Action<IGridObject> OnDestroyed { get; }
    public Action OnMoved { get; }

    private void Awake()
    {
        ResetGroupElements();
    }

    public void AddGroupElement(IGroupable groupable)
    {
        GroupElements.Add(groupable);
    }
    
    public void ResetGroupElements()
    {
        GroupElements = new HashSet<IGroupable>();
    }

    public void Damage(int damage)
    {
        OnDamaged?.Invoke(this);
        Destroy();
    }

    public void Destroy()
    {
        OnDestroyed?.Invoke(this);
    }

    public void MoveDown(IGridObject gridObject)
    {
        OnMoved?.Invoke();
    }
}