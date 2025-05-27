using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemySO : ScriptableObject
{
    public int id;
    public string enemyName;
    public int maxHealth;
    public float speed;
    public int dameAttack;
    public int moneyForDead;
    public float attackRange;
    public float attackCoolDown;

    // New properties for size and color
    public Vector3 enemyScale = Vector3.one;  // Default size is 1,1,1
    public Color enemyColor = Color.white;    // Default color is white
}
