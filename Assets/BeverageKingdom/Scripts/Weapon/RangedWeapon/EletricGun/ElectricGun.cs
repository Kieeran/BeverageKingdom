using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(AudioSource))]
public class ElectricGun : RangedWeapon
{
    [Header("Thiết lập tia điện")]
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private LineRenderer lineRenderer;     // Renderer để vẽ tia
    [SerializeField] private Texture[] textures;            // Dãy texture cho animation
    [SerializeField] private float fps = 30f;               // Tốc độ animation
    [SerializeField] private LayerMask enemyLayerMask;      // Chỉ detect Enemy

    [Header("Thiết lập damage")]
    [SerializeField] private int dame = 3;                // Sát thương trên mỗi lần tick
    [SerializeField] private float timeBetweenDamage = 0.2f;// Thời gian giữa 2 lần gây damage
    [SerializeField] private float timeDameElectric=0.3f;

    private float damageTimer;
    private float aniTimer;
    private int aniStep;
    private AudioSource electricSound;

    protected override void Awake()
    {
        // Lấy component nếu chưa gán trong Inspector
        lineRenderer = lineRenderer ?? GetComponent<LineRenderer>();
        electricSound = GetComponent<AudioSource>();

        // Thiết lập LineRenderer
        lineRenderer.positionCount = 2;    // Chỉ 2 điểm: start & end
        lineRenderer.enabled = false;      // Tắt ban đầu
    }
    public override void OnEnable()
    {
        damageTimer = 0f;
        aniTimer = 0f;
        aniStep = 0;
    }

    private void Update()
    {
        if (!lineRenderer.enabled) return;

        // Cập nhật timer gây damage
        damageTimer -= Time.deltaTime;
        // Vẽ và xử lý tia
        AnimateThunder();
        if (Input.GetMouseButtonUp(0))
        {
            StopShooting();
        }
    }

    public void StartShooting()
    {
        if (!electricSound.isPlaying)
            electricSound.Play();
        lineRenderer.enabled = true;
    }
    public void StopShooting()
    {
        electricSound.Stop();
        lineRenderer.enabled = false;
    }
    public override void Attack(Transform fireOrigin)
    {
        lineRenderer.enabled = true;
        Vector2 origin = fireOrigin.position;
        // Raycast 2D chỉ lên layer Enemy
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right, maxDistance, enemyLayerMask);

        Vector2 endPos = origin + Vector2.right * maxDistance;
        if (hit.collider != null)
        {
            endPos = hit.point;
            ElectricBetween2Enemy(endPos);
            if (damageTimer <= 0f)
            {
                if (hit.collider.TryGetComponent<Enemy>(out var enemy))
                {
                    enemy.Deduct(dame); 
                    ComboController.Instance.AddCombo();
                    enemy.Deduct(dame);
                }
                damageTimer = timeBetweenDamage;
            }
        }

        // Vẽ tia từ origin đến endPos
        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, endPos);
    }
    private void ElectricBetween2Enemy(Vector2 pointEnemy)
    {
        timeDameElectric -= Time.deltaTime;
        Collider2D[] collider2ds = Physics2D.OverlapCircleAll(pointEnemy, 3f,enemyLayerMask);
        if (collider2ds.Length == 0) return;
        List<GameObject> enemies = new List<GameObject>();
        foreach (Collider2D col in collider2ds)
        {
            if (col.tag == "Enemy")
                enemies.Add(col.gameObject);
        }
        if (enemies.Count == 0)
        {
            return;
        }
        int rad = Random.Range(0, enemies.Count);

        lineRenderer.SetPosition(2, enemies[rad].transform.position);
        if (timeDameElectric <= 0)
        {
            enemies[rad].GetComponentInChildren<Enemy>().Deduct(dame);
            timeDameElectric = 0.2f;
        }

    }
    private void AnimateThunder()
    {
        aniTimer += Time.deltaTime;
        if (aniTimer >= 1f / fps)
        {
            aniTimer = 0f;
            aniStep = (aniStep + 1) % textures.Length;
            lineRenderer.material.mainTexture = textures[aniStep];
        }
    }
}
