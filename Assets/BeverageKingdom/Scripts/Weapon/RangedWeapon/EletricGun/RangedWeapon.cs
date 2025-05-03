using UnityEngine;

public class RangedWeapon : Weapon
{
    [Header("Cooldown Settings")]
    [SerializeField] private float attackCooldown = 0.5f;

    private float lastAttackTime = -Mathf.Infinity;

    public override void Attack(Transform fireOrigin)
    {
        if (!CanShooting())
            return;               // Chưa đủ thời gian chờ, không bắn

        // Cập nhật thời điểm bắn
        lastAttackTime = Time.time;

        // Spawn viên đạn
        GameObject proj = ProjectileSpawner.Instance.Spawn(ProjectileSpawner.Bullet, fireOrigin.position, fireOrigin.rotation).gameObject;

        // ... (các thiết lập thêm cho projectile nếu cần)
    }

    private bool CanShooting()
    {
        return Time.time >= lastAttackTime + attackCooldown;
    }
}
