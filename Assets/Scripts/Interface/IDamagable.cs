using System;

public interface IDamagable
{
    Action<IGridObject> OnDamaged { get; }
    void Damage(int damage);
}