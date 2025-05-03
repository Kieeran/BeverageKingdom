using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  abstract class DeSpawn : TriBehaviour
{
    private void FixedUpdate()
    {
        DeSpawning();
    }
    protected abstract bool CanDespawn();
    
   
    protected virtual void DeSpawning()
    {
        if (!CanDespawn()) return;
        this.DeSpawnObj();
    }
    public virtual void DeSpawnObj()
    {
        // huy;
    }
}   
