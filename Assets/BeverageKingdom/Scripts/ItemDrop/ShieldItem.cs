using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : Item
{
    protected override void PickUp()
    {
        Player.instance.ActiveShield();

        Debug.Log("shield item");

        base.PickUp();
    }
}
