using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricSkill : ComboSkill
{
    private ElectricGun electricGun;

    [Header("Lightning Effect")]
    //  [SerializeField] private float dropHeight = 15f;       // khoảng cách sét rơi từ trên trời
    [SerializeField] private float damageRadius = 1f;      // Base radius for single target
    [SerializeField] private int damageAmount = 4;         // lượng damage gây ra
    [SerializeField] private int baseEnemyCount = 1;       // Number of enemies hit at low combo
    [SerializeField] private float maxComboRadius = 15f;   // Maximum radius at max combo

    private MainCanvas mainCanvas;
    protected override void Start()
    {
        base.Start();
        Debug.Log("ElectricSkill Start called");
        color = Color.yellow;
        electricGun = GetComponentInChildren<ElectricGun>();
        mainCanvas = MainCanvas.instance;
        if (mainCanvas != null)
        {
            Debug.Log("MainCanvas found, subscribing to OnComboMax");
            mainCanvas.comboBar.OnComboMax += ActivateComboSkill;
        }
        else
        {
            Debug.LogWarning("MainCanvas is null! Electric skill won't activate!");
        }
    }

    public override void ActivateComboSkill()
    {
        Debug.Log("ActivateComboSkill called on ElectricSkill");
        base.ActivateComboSkill();
        
        // Get current combo count
        int currentCombo = ComboController.Instance.CurrentCombo;
        int maxCombo = ComboController.Instance.maxCombo;
        
        // Calculate how many enemies to hit based on combo
        int enemiesToHit = Mathf.Min(baseEnemyCount + (currentCombo / 2), 10); // Cap at 10 enemies
        float currentRadius = Mathf.Lerp(damageRadius, maxComboRadius, (float)currentCombo / maxCombo);
        
        Debug.Log($"Combo: {currentCombo}, Hitting {enemiesToHit} enemies with radius {currentRadius}");

        // Get all enemies in the scene
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (allEnemies.Length == 0)
        {
            Debug.LogWarning("No enemies found in scene!");
            return;
        }

        // If we're at max combo, hit all enemies
        if (currentCombo >= maxCombo)
        {
            foreach (var enemyObj in allEnemies)
            {
                StrikeEnemy(enemyObj.transform.position, currentRadius, true);
            }
        }
        else
        {
            // Otherwise, hit random enemies
            List<GameObject> shuffledEnemies = new List<GameObject>(allEnemies);
            for (int i = shuffledEnemies.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                var temp = shuffledEnemies[i];
                shuffledEnemies[i] = shuffledEnemies[j];
                shuffledEnemies[j] = temp;
            }

            // Strike the first N enemies
            for (int i = 0; i < Mathf.Min(enemiesToHit, shuffledEnemies.Count); i++)
            {
                StrikeEnemy(shuffledEnemies[i].transform.position, 0.1f, false); // Use small radius for single target
            }
        }
    }

    private void StrikeEnemy(Vector3 position, float radius, bool isMaxCombo)
    {
        SoundManager.Instance?.PlaySound(SoundManager.Instance?.ThunderSoundFx, false);
        Debug.Log($"Lightning Strike Activated at position: {position}");

        EffectSpawner.instance.Spawn(EffectSpawner.Lightning, position + Vector3.up, Quaternion.identity);

        // Always use a small radius for single target hits
        float actualRadius = isMaxCombo ? radius : 0.1f;
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, actualRadius);
        int enemyCount = 0;
        
        foreach (var col in hits)
        {
            if (col.CompareTag("Enemy"))
            {
                var enemy = col.GetComponent<Enemy>();
                if (enemy != null)
                {
                    Debug.Log($"Striking enemy with {enemy.CurrentHealth} current health");
                    enemy.Deduct(damageAmount);
                    enemyCount++;
                    Debug.Log($"Dealt {damageAmount} damage to enemy at position {enemy.transform.position}");
                }
            }
        }
        Debug.Log($"Found {enemyCount} enemies in damage radius at position {position}");

        // Debug visualization
        Debug.DrawLine(position, position + Vector3.up * actualRadius, Color.red, 2f);
        Debug.DrawLine(position, position + Vector3.right * actualRadius, Color.red, 2f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = transform.position;
        Gizmos.DrawWireSphere(center, damageRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, maxComboRadius);
    }
}
