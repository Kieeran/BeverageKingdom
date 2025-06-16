using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpItem : Item
{
    [SerializeField] float _value;
    [SerializeField] float _buffDuration;
    protected override void PickUp()
    {
        Player.instance.SetSpeed(_value, _buffDuration);
        // base.PickUp();
    }
}