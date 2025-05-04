// RangedWeapon.cs
using UnityEngine;

public class RangedWeapon : Weapon
{
   // public float projectileSpeed = 20f;

    [SerializeField] private float fireCooldown = 0.5f;
    private float nextFireTime = 0f;
    protected override void Start()
    {
        fireCooldown = 0.01f;
    }

    public override void Attack()
    {
        if (Time.time < nextFireTime)
            return;

        nextFireTime = Time.time + fireCooldown;

        // Spawn viên đạn
        ProjectileSpawner.Instance
                       .Spawn(ProjectileSpawner.Bullet, fireOrigin.position, fireOrigin.rotation);

    }
    public override void PlayerUpgrade()
    {
        base.PlayerUpgrade();
        damage += 2;
    }
}
