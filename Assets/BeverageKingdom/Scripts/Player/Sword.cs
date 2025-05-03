using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MeleeWeapon
{
   /* public float attackDistance = 1f;
    public float attackRadius = 0.5f;
    public LayerMask enemyLayer;
    public int damage = 5;

    EffectSpawner effectSpawner;
    protected override void Start()
    {
        effectSpawner = EffectSpawner.instance;
    }
    public override void Attack(Transform fireOrigin)
    {   

        Vector2 origin = fireOrigin.position;
        effectSpawner.Spawn(EffectSpawner.Slash, origin, fireOrigin.rotation);

        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, attackRadius*2, enemyLayer);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.Deduct(damage);
                ComboController.Instance.AddCombo(); // nếu bạn muốn cộng combo khi đánh trúng
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying && transform == null) return;
        // Tính lại attackPoint dựa trên transform hiện tại
        Vector3 p = transform.position + transform.right * attackDistance;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(p, attackRadius);
    }*/
}
