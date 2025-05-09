using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricSkill : ComboSkill
{
    private ElectricGun electricGun;

    [Header("Lightning Effect")]
    // [SerializeField] private float dropHeight = 15f;       // khoảng cách sét rơi từ trên trời
    [SerializeField] private float damageRadius = 3f;      // bán kính gây damage
    // [SerializeField] private int damageAmount = 100;       // lượng damage gây ra

    protected override void Start()
    {
        base.Start();
        color = Color.yellow;
        electricGun = GetComponentInChildren<ElectricGun>();
    }

    protected override void ActivateComboSkill()
    {
        base.ActivateComboSkill();
        SoundManager.Instance?.PlaySound(SoundManager.Instance?.ThunderSoundFx, false);
        Debug.Log("Lightning Strike Activated!");
        // Transform target = EnemySpawner.Instance.randomPrefabHolder();

        // EffectSpawner.instance.Spawn(EffectSpawner.Lightning, target.position + Vector3.up, Quaternion.identity);

        // Collider2D[] hits = Physics2D.OverlapCircleAll(target.transform.position, damageRadius);
        // foreach (var col in hits)
        // {
        //     if (col.CompareTag("Enemy"))
        //     {
        //         var health = col.GetComponent<Enemy>();
        //         if (health != null)
        //         {
        //             health.Deduct(damageAmount);
        //         }
        //     }
        // }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        // Lấy vị trí cuối cùng của target; nếu chưa có target thì vẽ tại origin
        Vector3 center = transform.position;
        Gizmos.DrawWireSphere(center, damageRadius);
    }
}
