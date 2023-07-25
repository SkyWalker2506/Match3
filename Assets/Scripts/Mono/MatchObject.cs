using System;
using System.Collections.Generic;
using UnityEngine;

public class MatchObject : GridObject, IGroupable, IDamagable, IDestroyable, IMoveDown, IRenderByGroupSize
{
    [field: SerializeField] public int GroupIndex { get; private set; }
    public int GroupElementCount => GroupElements.Count;
    [field: SerializeField] public HashSet<IGridObject> GroupElements { get; set; }
    public int[] RendererLevelLimits { get; set; }
    public Action<IGridObject> OnDamaged { get; }
    public Action<IGridObject> OnDestroyed { get; }
    public Action OnMoved { get; }

    private void Awake()
    {
        ResetGroupElements();
    }

    public void AddGroupElement(IGroupable groupable)
    {
        GroupElements.Add(groupable.transform.GetComponent<IGridObject>());
    }
    
    public void ResetGroupElements()
    {
        GroupElements = new HashSet<IGridObject>();
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

    public override void UpdateSprite()
    {
        for (int i = 0; i < RendererLevelLimits.Length; i++)
        {
            if (RendererLevelLimits[i] > GroupElementCount)
            {
                SetSprite(i);
                return;
            }
        }
        SetSprite(RendererLevelLimits.Length);
    }

}