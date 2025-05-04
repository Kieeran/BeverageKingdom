using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceFreeze : ComboSkill
{
    [Header("C?u h?nh Ice Freeze")]
    [SerializeField] private int shardCount = 5;         // S? viên ð?n b?n ra
    [SerializeField] private float verticalSpacing = 1f; // Kho?ng cách gi?a các viên theo Y
    [SerializeField] private float shardLifetime = 3f;   // Th?i gian t?n t?i trý?c khi t? h?y

    protected override void ActivateComboSkill()
    {
        base.ActivateComboSkill();

        // Tính offset ð? hàng viên bãng cãn gi?a so v?i v? trí player
        float centerOffset = (shardCount - 1) / 2f;

        for (int i = 0; i < shardCount; i++)
        {
            // Tính v? trí spawn cho viên th? i
            float yOffset = (i - centerOffset) * verticalSpacing;
            Vector3 spawnPos = new Vector3(-4,0,0) + Vector3.up * yOffset;

            ProjectileSpawner.Instance.Spawn(ProjectileSpawner.Ice, spawnPos, Quaternion.identity);

            Debug.Log("asdasd");

        }
    }
}
