using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchObject : MonoBehaviour, IGridObject, IGroupable, IDamagable, IDestroyable
{
    public int WidthIndex { get; set; }
    public int HeightIndex { get; set; }
    public Image ObjectImage => image;
    public Sprite[] StateSprites => stateSprites;
    public int GroupIndex { get; }
    public int GroupCount => GroupElements.Count;
    public HashSet<IGroupable> GroupElements { get; private set; }
    public Action OnDamaged { get; }
    public Action OnDestroyed { get; }

    [SerializeField] private Image image;
    [SerializeField] private Sprite[] stateSprites;


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
        ObjectImage.sprite = stateSprites[index];
    }

    public void Destroy()
    {
        OnDestroyed?.Invoke();
    }

}