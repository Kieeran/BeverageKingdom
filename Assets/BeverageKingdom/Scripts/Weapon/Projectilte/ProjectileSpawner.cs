using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : Spawner
{
    public static ProjectileSpawner Instance { get; private set; }

    public static string Bullet = "Bullet";
    public static string Ice = "Ice";

    protected override void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
