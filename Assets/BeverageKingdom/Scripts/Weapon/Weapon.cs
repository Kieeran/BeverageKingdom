// Weapon.cs
using UnityEngine;

public abstract class Weapon : TriBehaviour
{
    [SerializeField] protected string weaponName;
    [SerializeField] protected int damage;
    [SerializeField] protected Transform fireOrigin;
    public abstract void Attack();
    public virtual void PlayerUpgrade()
    {
        damage += 5;
    }
}
