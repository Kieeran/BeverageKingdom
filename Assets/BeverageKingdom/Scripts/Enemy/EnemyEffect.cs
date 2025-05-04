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

    private IEnumerator FreezeCoroutine()
    {
        var enemy = GetComponent<Enemy>();
        float originalSpeed = enemy.EnemyMovement.MoveSpeed;
        enemy.EnemyMovement.MoveSpeed *= slowFactor;

        var sr = transform.Find("Model").GetComponentInChildren<SpriteRenderer>();
        Color originalColor = sr.color;

        float elapsed = 0f;
        while (elapsed < freezeDuration)
        {
            sr.color = flashColor;
            yield return new WaitForSeconds(flashInterval);
            sr.color = originalColor;
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval * 2f;
        }

        enemy.EnemyMovement.MoveSpeed = originalSpeed;
        sr.color = originalColor;
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
