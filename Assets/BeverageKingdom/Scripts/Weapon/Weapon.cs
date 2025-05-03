// Weapon.cs
using UnityEngine;

public abstract class Weapon : TriBehaviour
{
    [SerializeField] string weaponName;
    [SerializeField] int damage;
    public abstract void Attack(Transform fireOrigin);
}
