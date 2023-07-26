using System;

namespace Match3.Interface
{
    public interface IDamagable
    {
        Action<IGridObject> OnDamaged { get; }
        void Damage(int damage);
    }
}