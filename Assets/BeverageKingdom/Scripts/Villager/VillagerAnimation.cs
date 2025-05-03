using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerAnimation : MonoBehaviour
{
    public Action OnDoneHit;
    public void DoneHit()
    {
        OnDoneHit?.Invoke();
    }
}
