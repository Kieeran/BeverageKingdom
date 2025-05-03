using UnityEngine;

public class Enemy : TriBehaviour
{
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;
    public Rigidbody2D rb;
    public Animator ani;

    protected override void Awake()
    {
        currentHealth = maxHealth;
    }
    protected override void Start()
    {
        ani = transform.Find("Model").GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    public override void OnEnable()
    {
        base.OnEnable();
        currentHealth = maxHealth;
    }

    public void Deduct(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
