using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerAnimation : MonoBehaviour
{
    public Action OnDoneAttack;
    public void DoneHit()
    {
        OnDoneAttack?.Invoke();
    }
}
