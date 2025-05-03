// MeleeWeapon.cs
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [Header("Khoảng cách và Layer target")]
    public float range = 2f;
    public LayerMask hitLayers;
    int dame;

    public override void Attack(Transform fireOrigin)
    {
        // Chém gần: raycast thẳng theo hướng forward
        RaycastHit hit;
        if (Physics.Raycast(fireOrigin.position, fireOrigin.forward, out hit, range, hitLayers))
        {
            // Nếu target có Enemy, gây damage
            if (hit.collider.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.Deduct(dame);
            }
        }
        // Bạn có thể thêm effect, âm thanh ở đây
    }
}
