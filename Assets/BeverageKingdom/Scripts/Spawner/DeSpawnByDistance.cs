using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeSpawnByDistance : DeSpawn
{   
    protected float distanceLimit;
    protected float distance;

    protected override void Start()
    {

    }
    protected override bool CanDespawn()
    {
        distance = Vector2.Distance(Camera.main.transform.position, transform.parent.position);
        if (distance >= distanceLimit) return true;
        return false;
    }
}
