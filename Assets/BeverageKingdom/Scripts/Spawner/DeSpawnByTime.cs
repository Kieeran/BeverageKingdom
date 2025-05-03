using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeSpawnByTime : DeSpawn
{
    protected  float timeDespawn;
    protected float time = 0f;
    protected override void Start()
    {
        timeDespawn = 2f;
    }
    protected override bool CanDespawn()
    {
        time += Time.fixedDeltaTime;
        if (time > timeDespawn) return true;
        return false;
    }
    public override void OnEnable()
    {
        resetTime();
        base.OnEnable();
    }
    protected virtual void resetTime()
    {
        this.time = 0f;
    }
}
