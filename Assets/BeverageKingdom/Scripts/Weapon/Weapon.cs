// Weapon.cs
using UnityEngine;

public abstract class Weapon : TriBehaviour
{
    [SerializeField] protected string weaponName;
    [SerializeField] protected int damage;
    public abstract void Attack(Transform fireOrigin);
}
