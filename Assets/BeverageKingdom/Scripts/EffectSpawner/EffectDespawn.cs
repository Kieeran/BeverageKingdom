using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDespawn : DeSpawnByTime
{
    protected override void Start()
    {
        base.Start();
        timeDespawn = 2f;
    }
    public override void DeSpawnObj()
    {
        base.DeSpawnObj();
        EffectSpawner.instance.Despawm(transform);
    }
}
