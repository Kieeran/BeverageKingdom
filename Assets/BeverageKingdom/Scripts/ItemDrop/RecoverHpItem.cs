using UnityEngine;

public class RecoverHpItem : Item
{
    [SerializeField] float _value;

    protected override void PickUp()
    {
        Player.instance.RecoverHp(_value);
        // base.PickUp();
    }
}
