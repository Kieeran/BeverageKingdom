using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceFreeze : ComboSkill
{
    [Header("C?u h?nh Ice Freeze")]
    [SerializeField] private int shardCount = 5;         
    [SerializeField] private float verticalSpacing = 1f;

    private MainCanvas mainCanvas;
    protected override void Start()
    {
        base.Start();
        color = Color.blue;
        mainCanvas = MainCanvas.instance;
        mainCanvas.OnActiveSkill += ActivateComboSkill;
    }
    public override void ActivateComboSkill()
    {
        base.ActivateComboSkill();
        SoundManager.Instance?.PlaySound(SoundManager.Instance?.IceSound, false);
        float centerOffset = (shardCount - 1) / 2f;

        for (int i = 0; i < shardCount; i++)
        {
            float yOffset = (i - centerOffset) * verticalSpacing;
            Vector3 spawnPos = new Vector3(-4, 0, 0) + Vector3.up * yOffset;

            ProjectileSpawner.Instance.Spawn(ProjectileSpawner.Ice, spawnPos, Quaternion.identity);
        }
    }
}
