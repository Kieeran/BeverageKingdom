using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpItem : Item
{
    protected override void PickUp()
    {   
        StartCoroutine(Player.instance.SpeedUp(2f));
        base.PickUp();
    }
}