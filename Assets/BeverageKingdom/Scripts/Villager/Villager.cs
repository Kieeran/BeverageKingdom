using UnityEngine;

public class Villager : TriBehaviour
{
    [Header("Thiết lập máu")]
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;

    protected override void Awake()
    {
        currentHealth = maxHealth;
    }
    public override void OnEnable()
    {
        base.OnEnable();
        currentHealth = maxHealth;
    }

    public void Deduct(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} nhận {amount} damage, còn {currentHealth} HP");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} đã chết!");
        Destroy(gameObject);
    }
}
