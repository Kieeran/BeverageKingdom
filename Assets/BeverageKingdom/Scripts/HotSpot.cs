using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal; // BẮT BUỘC để dùng Light2D

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class HotSpot : MonoBehaviour
{
    [Header("HotSpot Settings")]
    public float radius = 2f;
    public float warnTime = 1f;
    public float growTime = 0.5f;
    public float activeTimeMin = 3f;
    public float activeTimeMax = 5f;

    [Header("Light Settings")]
    public float maxLightRadius = 3f;
    public float maxLightIntensity = 1.5f;

    private BoxCollider2D col;
    private SpriteRenderer sr;
    private Light2D spotLight;

    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        spotLight = GetComponentInChildren<Light2D>();

        col.enabled = false;
        sr.color = new Color(1f, 1f, 1f, 0f);
        transform.localScale = Vector3.zero;

        if (spotLight != null)
        {
            spotLight.intensity = 0f;
            spotLight.pointLightOuterRadius = 0f;
            spotLight.enabled = false;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(LifeCycle());
    }

    IEnumerator LifeCycle()
    {
        // // Warning phase
        // sr.color = new Color(1f, 1f, 0.5f, 0.5f); // Vàng mờ
        if (spotLight != null) spotLight.enabled = true;
        // yield return new WaitForSeconds(warnTime);

        // Grow phase
        float t = 0;
        while (t < growTime)
        {
            t += Time.deltaTime;
            float scale = Mathf.Lerp(0, radius, t / growTime);
            float lightRadius = Mathf.Lerp(0, maxLightRadius, t / growTime);
            float lightIntensity = Mathf.Lerp(0, maxLightIntensity, t / growTime);

            transform.localScale = new Vector3(scale, scale, 1f);
            if (spotLight != null)
            {
                spotLight.pointLightOuterRadius = lightRadius;
                spotLight.intensity = lightIntensity;
            }

            yield return null;
        }

        transform.localScale = new Vector3(radius, radius, 1f);
        col.enabled = true;
        sr.color = new Color(1f, 0.5f, 0f, 0.4f); // Cam mờ

        float activeTime = Random.Range(activeTimeMin, activeTimeMax);
        yield return new WaitForSeconds(activeTime);

        // Despawn phase
        col.enabled = false;
        t = 0;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            float scale = Mathf.Lerp(radius, 0, t / 0.5f);
            float lightRadius = Mathf.Lerp(maxLightRadius, 0, t / 0.5f);
            float lightIntensity = Mathf.Lerp(maxLightIntensity, 0, t / 0.5f);

            transform.localScale = new Vector3(scale, scale, 1f);
            if (spotLight != null)
            {
                spotLight.pointLightOuterRadius = lightRadius;
                spotLight.intensity = lightIntensity;
            }

            yield return null;
        }

        if (spotLight != null) spotLight.enabled = false;
        Destroy(gameObject); // hoặc dùng Object Pool
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            Player.instance.moveSpeed = 1; // giảm tốc
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {

            other.transform.parent.GetComponentInChildren<EnemyMovement>().MoveSpeed = 3; // tăng tốc
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player.instance.moveSpeed = 3; // giảm tốc
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            other.transform.parent.GetComponentInChildren<EnemyMovement>().MoveSpeed = 2; // hoặc giá trị mặc định
        }
    }

}
