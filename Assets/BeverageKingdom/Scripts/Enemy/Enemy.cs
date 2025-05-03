using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : TriBehaviour
{
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
