using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;      
    [SerializeField] private float maxLifetime = 5f; 

    [SerializeField] private int damage = 1;      
    
    private ComboController comboController;

    private float lifeTimer;
    private void Start()
    {
        comboController = ComboController.Instance;
    }

    private void OnEnable()
    {
        lifeTimer = maxLifetime;
    }

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
            Destroy(gameObject);
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "DetectionRange")
        {
            return;
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                SendDame(enemy);
            }

            Destroy(gameObject);
        }
    }

    protected virtual void SendDame(Enemy enemy)
    {
        enemy.Deduct(damage);
        comboController.AddCombo();
        // Destroy(this);
        ProjectileSpawner.Instance.Despawm(transform); EffectWhenDespwan();
    }

    protected virtual void EffectWhenDespwan()
    {

    }
}
