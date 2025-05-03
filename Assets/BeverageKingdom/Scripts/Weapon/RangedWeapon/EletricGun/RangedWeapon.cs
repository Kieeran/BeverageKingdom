// RangedWeapon.cs
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Ranged Weapon")]
public class RangedWeapon : Weapon
{
    [Header("Prefab đạn và tốc độ bắn")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 20f;

    public override void Attack(Transform fireOrigin)
    {
        // Spawn viên đạn
        GameObject proj = Instantiate(projectilePrefab, fireOrigin.position, fireOrigin.rotation);
        // Gán velocity
        if (proj.TryGetComponent<Rigidbody>(out var rb))
            rb.velocity = fireOrigin.forward * projectileSpeed;

       /* if (proj.TryGetComponent<Projectile>(out var p))
            p.damage = damage;*/
    }
}
