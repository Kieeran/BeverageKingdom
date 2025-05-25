using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverHpItem : Item
{
    protected override void PickUp()
    {
        Player.instance.RecoverHp(5);
        base.PickUp();
    }
}
