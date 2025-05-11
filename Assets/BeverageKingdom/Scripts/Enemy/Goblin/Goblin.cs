using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy
{
    protected override void Start()
    {
        base.Start();
        CurrentHealth = 15;
    }
    protected override void Init()
    {
        if (enemyData == null)
        {
            AttackRange = 2.3f;
            AttackCoolDown = 1.5f;
            Damage = 4;
            maxHealth = 14f;
        }
        else
        {
            AttackRange = enemyData.attackRange;
            AttackCoolDown = enemyData.attackCoolDown;
            Damage = enemyData.dameAttack;
        }
    }
}
