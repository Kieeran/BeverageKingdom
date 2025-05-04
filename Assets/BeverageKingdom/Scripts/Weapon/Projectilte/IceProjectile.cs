using System.Collections;
using UnityEngine;

public class IceProjectile : Projectile
{
    // Khoảng cách giữa mỗi lần nhấp nháy

    protected override void SendDame(Enemy enemy)
    {
        base.SendDame(enemy);
        EffectSpawner.instance.Spawn(EffectSpawner.IceBreak, transform.position, Quaternion.identity);

        EnemyEffect effect = enemy.GetComponent<EnemyEffect>();
        if (effect != null)
            effect.ApplyFreezeEffect();
    }

}
