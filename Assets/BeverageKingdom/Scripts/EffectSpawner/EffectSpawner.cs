using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : Spawner
{
    public static EffectSpawner instance;

    public static string Slash = "Slash";
    public static string Lightning = "Lightning";
    public static string IceBreak = "IceBreak";
    public static string LevelUp = "LevelUp";
    protected override void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
