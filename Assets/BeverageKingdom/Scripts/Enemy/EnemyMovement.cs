using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float MoveSpeed;
    [HideInInspector]
    public Transform Target;
    [HideInInspector]
    public bool IsEntityInRange;
    public DetectionRange DetectionRange;

    public Action<int> OnStageChange;

    public bool IsWalking;
    public bool IsIdling;
    public bool IsAttacking;

    void Awake()
    {
        DetectionRange.OnInRange += SetEntityInRange;
        DetectionRange.OnOutRange += SetEntityOutRange;

        SetStage(2);
    }

    void Update()
    {
        // SetStage(1);
        // if (Target != null && IsEntityInRange == true)
        // {
        //     Vector3 directionToPlayer = (Target.position - transform.position).normalized;
        //     transform.parent.position += directionToPlayer * MoveSpeed * Time.deltaTime;
        //     SetStage(2);
        // }
        // else
        // {
        //     transform.parent.position += Vector3.left * MoveSpeed * Time.deltaTime;
        //     SetStage(2);
        // }
    }

    public void Walk()
    {
        Vector3 directionToPlayer = (Target.position - transform.position).normalized;
        transform.parent.position += MoveSpeed * Time.deltaTime * directionToPlayer;
    }

    public void GoToKindom()
    {
        transform.parent.position += MoveSpeed * Time.deltaTime * Vector3.left;
    }

    public void SetStage(int index)
    {
        if (index == 1 && IsIdling == true) return;
        if (index == 2 && IsWalking == true) return;
        if (index == 3 && IsAttacking == true) return;

        IsIdling = index == 1;
        IsWalking = index == 2;
        IsAttacking = index == 3;

        OnStageChange?.Invoke(index);
    }

    void SetEntityInRange(Transform transform)
    {
        IsEntityInRange = true;
        Target = transform;
    }

    void SetEntityOutRange()
    {
        IsEntityInRange = false;
        Target = null;
    }
}