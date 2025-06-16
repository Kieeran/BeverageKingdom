using System.Collections;
using UnityEngine;

public class EnemyEffect : MonoBehaviour
{
    [Header("Freeze Effect Settings")]
    [SerializeField] private float freezeDuration = 2f;
    [SerializeField] private float slowFactor = 0.5f;
    [SerializeField] private Color flashColor = Color.cyan;
    [SerializeField] private float flashInterval = 0.1f;

    [Header("Knockback Settings")]
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackDuration = 0.2f;

    private Coroutine freezeRoutine;
    private Coroutine knockbackRoutine;

    SpriteRenderer _spriteRenderer;
    Color _originalColor;

    void Awake()
    {
        _spriteRenderer = transform.Find("Model").GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        _originalColor = _spriteRenderer.color;
    }

    public void ApplyFreezeEffect()
    {
        if (freezeRoutine != null) StopCoroutine(freezeRoutine);
        freezeRoutine = StartCoroutine(FreezeCoroutine());
    }

    /// <summary>
    /// Áp dụng knockback, tự lấy vị trí player
    /// </summary>
    public void ApplyKnockBack()
    {
        if (knockbackRoutine != null) StopCoroutine(knockbackRoutine);
        knockbackRoutine = StartCoroutine(KnockBackCoroutine());
    }
    public void ApplyHitEffect()
    {
        StartCoroutine(HitEffectCoroutine());
    }

    private IEnumerator HitEffectCoroutine()
    {
        if (_spriteRenderer == null) yield break;

        _spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.1f); // Thời gian đổi màu đỏ

        _spriteRenderer.color = _originalColor;
    }

    private IEnumerator FreezeCoroutine()
    {
        var enemy = GetComponent<Enemy>();
        float originalSpeed = enemy.EnemyMovement.MoveSpeed;
        enemy.EnemyMovement.MoveSpeed *= slowFactor;

        float elapsed = 0f;
        while (elapsed < freezeDuration)
        {
            _spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashInterval);
            _spriteRenderer.color = _originalColor;
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval * 2f;
        }

        enemy.EnemyMovement.MoveSpeed = originalSpeed;
        _spriteRenderer.color = _originalColor;
        freezeRoutine = null;
    }

    private IEnumerator KnockBackCoroutine()
    {
        var rb = GetComponent<Rigidbody2D>();
        if (rb == null) yield break;

        // Lấy vị trí Player
        Vector2 playerPos = Player.instance.transform.position;
        // Hướng đẩy: từ Player đến Enemy
        Vector2 direction = ((Vector2)transform.position - playerPos).normalized;
        rb.velocity = direction * knockbackForce;

        yield return new WaitForSeconds(knockbackDuration);

        rb.velocity = Vector2.zero;
        knockbackRoutine = null;
    }
}