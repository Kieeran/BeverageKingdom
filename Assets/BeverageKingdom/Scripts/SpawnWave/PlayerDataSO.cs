// PlayerDataSO.cs
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData")]
public class PlayerDataSO : ScriptableObject
{
    public int maxHealth;
    public float moveSpeed;
    public int dameSword;
    public int dameGun;
    public int dameIce;

    [Header("Các thông số nâng cao")]
    public float attackCooldown;
    public float defense;

}
