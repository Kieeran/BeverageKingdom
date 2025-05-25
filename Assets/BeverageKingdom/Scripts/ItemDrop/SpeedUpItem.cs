using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpItem : Item
{
    protected override void PickUp()
    {   
        StartCoroutine(Player.instance.SetSpeed(5f));
        base.PickUp();
    }
}