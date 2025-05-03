using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(AudioSource))]
public class ElectricGun : RangedWeapon
{
    [Header("Thiết lập tia điện")]
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Texture[] textures;
    [SerializeField] private float fps = 30f;
    [SerializeField] private LayerMask enemyLayerMask;

    [Header("Thiết lập damage")]
    [SerializeField] private int dame = 3;
    [SerializeField] private float timeBetweenDamage = 0.2f;
    [SerializeField] private float timeDameElectric = 0.3f;

    [Header("Giới hạn thời gian bắn")]
    [SerializeField] private float shootDuration = 1f;  // bắn tối đa 1s
    private float shootTimer;
    private bool isShooting;

    private float damageTimer;
    private float aniTimer;
    private int aniStep;
    private AudioSource electricSound;

    protected override void Awake()
    {
        lineRenderer = lineRenderer ?? GetComponent<LineRenderer>();
        electricSound = GetComponent<AudioSource>();
        lineRenderer.positionCount = 3;    // 3 điểm cho chain
        lineRenderer.useWorldSpace = true;
        lineRenderer.enabled = false;
    }

    public override void OnEnable()
    {
        damageTimer = 0f;
        aniTimer = 0f;
        aniStep = 0;
        isShooting = false;
    }

    private void Update()
    {
        if (isShooting)
        {
            // Quản lý thời gian bắn
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                StopShooting();
                isShooting = false;
                return;
            }

            // Cập nhật timer gây damage
            damageTimer -= Time.deltaTime;
            // Vẽ & xử lý tia
            AnimateThunder();

            // Tự tắt khi thả nút chuột
            if (Input.GetMouseButtonUp(0))
            {
                StopShooting();
                isShooting = false;
            }
        }
    }

    public void StartShooting()
    {
        if (!electricSound.isPlaying)
            electricSound.Play();

        lineRenderer.enabled = true;
        shootTimer = shootDuration;
        isShooting = true;
    }

    public void StopShooting()
    {
        electricSound.Stop();
        lineRenderer.enabled = false;
    }

    public override void Attack()
    {
        // Nếu chưa đang bắn, khi lần đầu Attack kích hoạt bắn
        if (!isShooting)
            StartShooting();

        if (!isShooting)
            return;  // nếu vừa hết thời gian, không chạy Attack nữa

        Vector2 origin = fireOrigin.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right, maxDistance, enemyLayerMask);
        Vector2 endPos = origin + Vector2.right * maxDistance;

        if (hit.collider != null)
        {
            endPos = hit.point;
            //ElectricBetween2Enemy(endPos);

            if (damageTimer <= 0f)
            {
                if (hit.collider.TryGetComponent<Enemy>(out var enemy))
                {
                    enemy.Deduct(dame);
                    ComboController.Instance.AddCombo();
                }
                damageTimer = timeBetweenDamage;
            }
        }

        // Vẽ tia từ origin đến endPos
        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, endPos);
        // Chain point (nếu có) sẽ được set bên ElectricBetween2Enemy
    }

    private void ElectricBetween2Enemy(Vector2 pointEnemy)
    {
        timeDameElectric -= Time.deltaTime;
        Collider2D[] cols = Physics2D.OverlapCircleAll(pointEnemy, 3f, enemyLayerMask);
        var enemies = new List<GameObject>();
        foreach (var col in cols)
                enemies.Add(col.gameObject);

        if (enemies.Count == 0) return;

        int idx = Random.Range(0, enemies.Count);
        Vector3 chainPos = enemies[idx].transform.position;
        lineRenderer.SetPosition(2, chainPos);

        if (timeDameElectric <= 0f)
        {
            enemies[idx].GetComponentInChildren<Enemy>().Deduct(dame);
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
