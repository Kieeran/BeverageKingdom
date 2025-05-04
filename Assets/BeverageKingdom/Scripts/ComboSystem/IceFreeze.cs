using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceFreeze : ComboSkill
{
    [Header("C?u h?nh Ice Freeze")]
    [SerializeField] private int shardCount = 5;         // S? vi�n �?n b?n ra
    [SerializeField] private float verticalSpacing = 1f; // Kho?ng c�ch gi?a c�c vi�n theo Y
    // [SerializeField] private float shardLifetime = 3f;   // Th?i gian t?n t?i tr�?c khi t? h?y

    protected override void Start()
    {
        base.Start();
        color = Color.blue;
    }
    protected override void ActivateComboSkill()
    {
        base.ActivateComboSkill();

        // T�nh offset �? h�ng vi�n b�ng c�n gi?a so v?i v? tr� player
        float centerOffset = (shardCount - 1) / 2f;

        for (int i = 0; i < shardCount; i++)
        {
            // T�nh v? tr� spawn cho vi�n th? i
            float yOffset = (i - centerOffset) * verticalSpacing;
            Vector3 spawnPos = new Vector3(-4,0,0) + Vector3.up * yOffset;

            ProjectileSpawner.Instance.Spawn(ProjectileSpawner.Ice, spawnPos, Quaternion.identity);


        }
    }
}
