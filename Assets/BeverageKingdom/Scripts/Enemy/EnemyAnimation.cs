using System;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public Action OnDoneAttack;
    public void DoneHit()
    {
        OnDoneAttack?.Invoke();
    }
}
