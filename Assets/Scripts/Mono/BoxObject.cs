using System;
using UnityEngine;

public class BoxObject : GridObject, IHaveHealth, IDamagableFromNeighbour, IDestroyable
{
    [field: SerializeField]  public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public Action<IGridObject> OnDamaged { get; }
    public Action<IGridObject> OnDestroyed { get; }

    private void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    public void Damage(int damage)
    {
        CurrentHealth =- damage;
        OnDamaged?.Invoke(this);
        if (CurrentHealth <= 0)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        OnDestroyed?.Invoke(this);
    }

    public override void UpdateSprite()
    {
        SetSprite(CurrentHealth-1);
    }
}