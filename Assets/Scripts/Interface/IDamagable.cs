using System;

public interface IDamagable
{
    Action<IGridObject> OnDamaged { get; }
    void Damage(int damage);
}

public interface IDamagableFromNeighbour : IDamagable
{
    
}