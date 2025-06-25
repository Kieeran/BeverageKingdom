using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricSkill : ComboSkill
{
    private ElectricGun electricGun;

    [Header("Lightning Effect")]
    //  [SerializeField] private float dropHeight = 15f;       // khoảng cách sét rơi từ trên trời
    [SerializeField] private float damageRadius = 1f;      // Bán kính sét đánh cơ bản cho 1 quái
    [SerializeField] private int damageAmount = 4;         // Lượng damage gây ra
    [SerializeField] private int baseEnemyCount = 1;       // Số lượng quái đánh tại ngưỡng thấp
    [SerializeField] private float maxComboRadius = 15f;   // Bán kính sét đánh tối đa tại ngưỡng cao

    private PlayCanvas playCanvas;
    private ComboBar comboBar;

    protected override void Start()
    {
        base.Start();
        color = Color.yellow;
        electricGun = GetComponentInChildren<ElectricGun>();
        playCanvas = PlayCanvas.Instance;
        if (playCanvas != null)
        {
            comboBar = playCanvas.comboBar;
            comboBar.OnComboMax += ActivateComboSkill;
        }
        else
        {
        }
    }

    public override void ActivateComboSkill()
    {
        base.ActivateComboSkill();

        // Lấy số combo hiện tại
        int currentCombo = ComboController.Instance.CurrentCombo;
        int maxCombo = ComboController.Instance.maxCombo;

        // Tính toán số lượng quái để đánh dựa trên ngưỡng combo
        int enemiesToHit;
        if (currentCombo >= comboBar.HighThreshold)
        {
            enemiesToHit = currentCombo / 2; // Số lượng enemy sét giật tối đa tại ngưỡng cao
        }
        else if (currentCombo >= comboBar.LowThreshold)
        {
            enemiesToHit = currentCombo / 5; // Số lượng enemy sét giật tại ngưỡng trung bình
        }
        else
        {
            enemiesToHit = baseEnemyCount; // Số lượng enemy sét giật cơ bản dưới ngưỡng thấp
        }

        float currentRadius = Mathf.Lerp(damageRadius, maxComboRadius, (float)currentCombo / maxCombo);

        // Lấy tất cả quái trong scene
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (allEnemies.Length == 0)
        {
            return;
        }

        // Nếu combo đạt max, đánh tất cả quái
        if (currentCombo >= maxCombo)
        {
            foreach (var enemyObj in allEnemies)
            {
                StrikeEnemy(enemyObj.transform.position, currentRadius, true);
            }
        }
        else
        {
            // Nếu không, đánh ngẫu nhiên quái
            List<GameObject> shuffledEnemies = new List<GameObject>(allEnemies);
            for (int i = shuffledEnemies.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                var temp = shuffledEnemies[i];
                shuffledEnemies[i] = shuffledEnemies[j];
                shuffledEnemies[j] = temp;
            }

            // Đánh quái đầu tiên N
            for (int i = 0; i < Mathf.Min(enemiesToHit, shuffledEnemies.Count); i++)
            {
                StrikeEnemy(shuffledEnemies[i].transform.position, 0.1f, false); // Sử dụng bán kính nhỏ để sét đánh chính xác hitbox 1 quái
            }
        }
    }

    private void StrikeEnemy(Vector3 position, float radius, bool isMaxCombo)
    {
        SoundManager.Instance?.PlayAudio(SoundManager.Instance?.ThunderSound, false);

        EffectSpawner.instance.Spawn(EffectSpawner.Lightning, position + Vector3.up, Quaternion.identity);

        // Luôn sử dụng bán kính nhỏ để sét đánh chính xác hitbox 1 quái
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
                    enemy.Deduct(damageAmount);
                    enemyCount++;
                }
            }
        }

        // Hiện hitbox sét đánh
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
