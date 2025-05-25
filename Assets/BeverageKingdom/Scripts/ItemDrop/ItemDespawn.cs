using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDespawn : DeSpawnByTime
{
    protected override void Start()
    {
        base.Start();
        timeDespawn = 4f; // Set the time after which the item will despawn
    }
    public override void DeSpawnObj()
    {
        base.DeSpawnObj();
        ItemSpawner.Instance.Despawm(transform);
    }
}
