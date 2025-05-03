using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricSkill : ComboSkill
{   
    private ElectricGun electricGun;
    private void Start()
    {
        electricGun = GetComponentInChildren<ElectricGun>();
    }
    protected override void ActivateComboSkill()
    {
        base.ActivateComboSkill();
        electricGun.Attack(Player.instance.transform);
    }
}
