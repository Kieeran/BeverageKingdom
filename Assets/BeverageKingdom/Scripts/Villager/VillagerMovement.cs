using System;
using UnityEngine;

public class VillagerMovement : MonoBehaviour
{
    public float MoveSpeed;
    [HideInInspector]
    public Transform Target;
    [HideInInspector]
    public bool IsEntityInRange;
    public DetectionRange DetectionRange;
    public Villager Villager;

    public Action<int> OnStageChange;

    [HideInInspector]
    public bool IsWalking;
    [HideInInspector]
    public bool IsIdling;
    [HideInInspector]
    public bool IsAttacking;
    [HideInInspector]
    public bool IsDead;

    void Awake()
    {
        DetectionRange.OnInRange += SetEntityInRange;
        DetectionRange.OnOutRange += SetEntityOutRange;

        SetStage(1);
    }

    public void Walk()
    {
        Vector3 directionToPlayer = (Target.position - transform.position).normalized;
        transform.parent.position += directionToPlayer * MoveSpeed * Time.deltaTime;
        SetStage(2);
    }

    public void SetStage(int index)
    {
        if (index == 1 && IsIdling == true) return;
        if (index == 2 && IsWalking == true) return;
        if (index == 3 && IsAttacking == true) return;
        if (index == 4 && IsDead == true) return;

        IsIdling = index == 1;
        IsWalking = index == 2;
        IsAttacking = index == 3;
        IsDead = index == 4;

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