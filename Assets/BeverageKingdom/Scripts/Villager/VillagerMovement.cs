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

    public Action<int> OnStageChange;
    
    [HideInInspector]
    public bool IsWalking;
    [HideInInspector]
    public bool IsIdling;
    [HideInInspector]
    public bool IsHitting;

    void Awake()
    {
        DetectionRange.OnInRange += SetEntityInRange;
        DetectionRange.OnOutRange += SetEntityOutRange;

        SetStage(1);
    }

    void Update()
    {
        if (Target != null && IsEntityInRange == true)
        {
            Vector3 directionToPlayer = (Target.position - transform.position).normalized;
            transform.parent.position += directionToPlayer * MoveSpeed * Time.deltaTime;
            SetStage(2);
        }
        else
        {
            SetStage(1);
        }
    }

    void SetStage(int index)
    {
        if (index == 1 && IsIdling == true) return;
        if (index == 2 && IsWalking == true) return;
        if (index == 3 && IsHitting == true) return;

        IsIdling = index == 1;
        IsWalking = index == 2;
        IsHitting = index == 3;

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