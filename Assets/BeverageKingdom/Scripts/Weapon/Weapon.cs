// Weapon.cs
using UnityEngine;

public abstract class Weapon : TriBehaviour
{
    [Header("Tên và sát thương cơ bản")]
    public string weaponName;
    public int damage;
    public abstract void Attack(Transform fireOrigin);
}
