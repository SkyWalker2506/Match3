using System;

public interface IDamagable
{
    Action OnDamaged { get; }
    void Damage(int damage);
}