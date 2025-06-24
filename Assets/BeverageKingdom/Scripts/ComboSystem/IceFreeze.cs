using UnityEngine;

public class IceFreeze : ComboSkill
{
    [Header("Cau hinh Ice Freeze")]
    [SerializeField] private int shardCount = 5;
    [SerializeField] private float verticalSpacing = 1f;

    private PlayCanvas playCanvas;
    protected override void Start()
    {
        base.Start();
        color = Color.blue;
        playCanvas = PlayCanvas.Instance;
        playCanvas.OnActiveSkill += ActivateComboSkill;
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
