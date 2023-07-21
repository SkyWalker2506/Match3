using System;
using System.Collections.Generic;
using UnityEngine;

public class MatchObject : MonoBehaviour, IGridObject, IGroupable, IDamagable, IDestroyable
{
    public int WidthIndex { get; set; }
    public int HeightIndex { get; set; }
    [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
    [field: SerializeField] public Sprite[] StateSprites { get; private set; }
    [field: SerializeField] public int GroupIndex { get; private set; }
    public int GroupCount => GroupElements.Count;
    public HashSet<IGroupable> GroupElements { get; private set; }
    public Action OnDamaged { get; }
    public Action OnDestroyed { get; }



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
        OnDamaged?.Invoke();
        Destroy();
    }

    public void SetSprite(int index)
    {
        SpriteRenderer.sprite = StateSprites[index];
    }

    public void Destroy()
    {
        OnDestroyed?.Invoke();
    }

}