// MeleeWeapon.cs
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public float attackLength = 1f;
    public float attackWidth = 2f;
    public LayerMask hitLayers;

    public float attackCooldown = 0.5f;
    private float nextAttackTime = 0f;

    public override void Attack(Transform fireOrigin)
    {
        if (Time.time < nextAttackTime) return;
        nextAttackTime = Time.time + attackCooldown;

        Vector2 origin = transform.position + Vector3.right;
        Vector2 forward = fireOrigin.right.normalized;
        Vector2 boxCenter = origin + forward * (attackLength * 0.5f);

        Vector2 boxSize = new Vector2(attackLength, attackWidth);

        float angle = fireOrigin.eulerAngles.z;
        EffectSpawner.instance.Spawn(EffectSpawner.Slash, boxCenter, fireOrigin.rotation);
        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, angle, hitLayers);

        foreach (var hit in hits)
        {

            if (hit.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.Deduct(damage);

                Debug.Log(damage);

                ComboController.Instance.AddCombo();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (transform == null) return;

        Vector2 origin = transform.position + Vector3.right;
        Vector2 forward = transform.right.normalized;
        Vector2 boxCenter = origin + forward * (attackLength * 0.5f);
        Vector2 boxSize = new Vector2(attackLength, attackWidth);
        float angle = transform.eulerAngles.z;

        // Lưu ma trận gốc
        Matrix4x4 oldMat = Gizmos.matrix;

        Gizmos.matrix = Matrix4x4.TRS(boxCenter, Quaternion.Euler(0, 0, angle), Vector3.one);
        Gizmos.color = Color.red;
        // Vẽ hộp có center=(0,0), size=boxSize
        Gizmos.DrawWireCube(Vector3.zero, boxSize);

        // Khôi phục ma trận
        Gizmos.matrix = oldMat;
    }
}
