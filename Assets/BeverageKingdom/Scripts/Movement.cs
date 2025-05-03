using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool IsEnemy;
    public float MoveSpeed;
    [HideInInspector]
    public Transform Target;
    [HideInInspector]
    public bool IsEntityInRange;
    public DetectionRange DetectionRange;

    void Awake()
    {
        DetectionRange.OnInRange += SetEntityInRange;
        DetectionRange.OnOutRange += SetEntityOutRange;
    }

    void Update()
    {
        if (Target != null && IsEntityInRange == true)
        {
            Vector3 directionToPlayer = (Target.position - transform.position).normalized;
            transform.parent.position += directionToPlayer * MoveSpeed * Time.deltaTime;
        }
        else
        {
            if (IsEnemy)
                transform.parent.position += Vector3.left * MoveSpeed * Time.deltaTime;
        }
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