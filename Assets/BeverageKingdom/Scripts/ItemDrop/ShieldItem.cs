using UnityEngine;

public class ShieldItem : Item
{
    [SerializeField] float _buffDuration;

    protected override void PickUp()
    {
        Player.instance.ActiveShield(_buffDuration);
        // base.PickUp();
    }
}